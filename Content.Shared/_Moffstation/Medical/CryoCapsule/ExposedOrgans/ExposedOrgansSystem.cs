using System.Linq;
using Content.Shared.Body;
using Content.Shared.Examine;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Verbs;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;

namespace Content.Shared._Moffstation.Medical.CryoCapsule.ExposedOrgans;

/// <summary>
/// This handles...
/// </summary>
public sealed class ExposedOrgansSystem : EntitySystem
{
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    /// <inheritdoc/>
    public override void Initialize()
    {
        SubscribeLocalEvent<ExposedOrgansComponent, ComponentInit>(OnComponentInit);
        SubscribeLocalEvent<ExposedOrgansComponent, GetVerbsEvent<AlternativeVerb>>(OnGetAlternativeVerbs);
        SubscribeLocalEvent<ExposedOrgansComponent, ExaminedEvent>(OnExamined);

        SubscribeLocalEvent<ExposedOrgansComponent, InteractUsingEvent>(OnInteractUsing);
    }

    private void OnComponentInit(Entity<ExposedOrgansComponent> ent, ref ComponentInit args)
    {
        var bodyComp = EnsureComp<BodyComponent>(ent);

        if (bodyComp.Organs?.ContainedEntities is not { } organs)
            return;

        var categories = ent.Comp.ExposedOrgans.Values;

        foreach (var organ in bodyComp.Organs.ContainedEntities)
        {
            if (! TryComp<OrganComponent>(organ, out var organComp) ||
                organComp.Category is null ||
                ! categories.Contains<ProtoId<OrganCategoryPrototype>>(organComp.Category.Value))
                continue;

            ent.Comp.PresentOrgans[organComp.Category.Value] = organ;
        }
    }

    private void OnGetAlternativeVerbs(Entity<ExposedOrgansComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        var user = args.User;

        foreach (var category in ent.Comp.ExposedOrgans)
        {
            if (!ent.Comp.PresentOrgans.TryGetValue(category.Value, out var present) || present == null)
                continue;

            args.Verbs.Add(new AlternativeVerb
            {
                Text = category.Key,
                Category = VerbCategory.Eject,
                Act = () => TryEjectOrgan(ent, category.Value, user, present.Value, true),
            });
        }
    }

    // todo
    private void OnExamined(Entity<ExposedOrgansComponent> ent, ref ExaminedEvent args)
    {
            args.PushMarkup("organs");
    }

    private void OnInteractUsing(Entity<ExposedOrgansComponent> ent, ref InteractUsingEvent args)
    {
        if (!TryComp<OrganComponent>(args.Used, out var organ) ||
            organ.Category is not { } category ||
            !ent.Comp.ExposedOrgans.ContainsValue(category))
            return;

        var success = (!ent.Comp.PresentOrgans.TryGetValue(category, out var present) || present == null)
            ? TryInsertOrgan(ent, category, args.User, args.Used)
            : TrySwapOrgan(ent, category, args.User, args.Used, present.Value);

        if (success)
            _audio.PlayPredicted(ent.Comp.InteractionSound, ent, args.User);
    }

    #region private implementation

    private bool TryInsertOrgan(Entity<ExposedOrgansComponent> ent,
        ProtoId<OrganCategoryPrototype> category,
        EntityUid? user,
        EntityUid organ,
        bool noisy = false)
    {
        if (!TryComp<BodyComponent>(ent, out var body) ||
            body.Organs == null ||
            !_container.Insert(organ, body.Organs))
            return false;

        ent.Comp.PresentOrgans[category] = organ;
        if (noisy)
            _audio.PlayPredicted(ent.Comp.InteractionSound, ent, user);
        return true;
    }

    private bool TryEjectOrgan(Entity<ExposedOrgansComponent> ent,
        ProtoId<OrganCategoryPrototype> category,
        EntityUid? user,
        EntityUid organ,
        bool noisy = false)
    {
        if (!TryComp<BodyComponent>(ent, out var body) ||
            body.Organs is null ||
            !_container.Remove(organ, body.Organs))
            return false;

        if (user is not null)
            _hands.TryPickup(user.Value, organ);

        ent.Comp.PresentOrgans[category] = null;
        if (noisy)
            _audio.PlayPredicted(ent.Comp.InteractionSound, ent, user);
        return true;
    }

    private bool TrySwapOrgan(Entity<ExposedOrgansComponent> ent,
        ProtoId<OrganCategoryPrototype> category,
        EntityUid? user,
        EntityUid toInsert,
        EntityUid toEject,
        bool noisy = false)
    {
        if (!TryEjectOrgan(ent, category, user, toEject) ||
            !TryInsertOrgan(ent, category, user, toInsert))
            return false;

        if (user is not null)
            _hands.TryPickup(user.Value, toEject);

        ent.Comp.PresentOrgans[category] = toInsert;
        if (noisy)
            _audio.PlayPredicted(ent.Comp.InteractionSound, ent, user);
        return true;
    }

    #endregion
}
