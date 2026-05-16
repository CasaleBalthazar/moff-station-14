using System.Linq;
using Content.Shared._Moffstation.Body.Components;
using Content.Shared.Body;
using Content.Shared.StatusEffectNew;
using Robust.Shared.Prototypes;

namespace Content.Shared._Moffstation.Body.Systems;

/// <summary>
/// This handles...
/// </summary>
public sealed class FunctionalOrgansSystem : EntitySystem
{
    [Dependency] private readonly StatusEffectsSystem _statusEffects = default!;

    /// <inheritdoc/>
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FunctionalOrgansComponent, ComponentStartup>(OnComponentStartup);

        SubscribeLocalEvent<FunctionalOrgansComponent, OrganInsertedIntoEvent>(OnOrganInserted);
        SubscribeLocalEvent<FunctionalOrgansComponent, OrganRemovedFromEvent>(OnOrganRemoved);
    }

    public void OnComponentStartup(Entity<FunctionalOrgansComponent> ent, ref ComponentStartup args)
    {
        if (!TryComp<BodyComponent>(ent, out var body) || body.Organs is null)
            return;

        foreach (var organ in body.Organs.ContainedEntities)
        {
            if (!TryComp<OrganComponent>(ent, out var comp) || comp.Category is not {} category)
                continue;

            ent.Comp.CategoriesPresent.Add(category);
        }

        foreach (var group in ent.Comp.OrganGroups.Values)
        {
            UpdateEffects(group, ent, ent.Comp.CategoriesPresent, initial:true);
        }
    }

    public void OnOrganInserted(Entity<FunctionalOrgansComponent> ent, ref OrganInsertedIntoEvent ev)
    {
        if (!TryComp<OrganComponent>(ev.Organ, out var organ) ||
            organ.Category is not { } category)
            return;

        ent.Comp.CategoriesPresent.Add(category);
        foreach (var group in ent.Comp.OrganGroups.Values)
        {
            UpdateEffects(group, ent, ent.Comp.CategoriesPresent);
        }
    }

    public void OnOrganRemoved(Entity<FunctionalOrgansComponent> ent, ref OrganRemovedFromEvent ev)
    {
        if (!TryComp<OrganComponent>(ev.Organ, out var organ) ||
            organ.Category is not { } category)
            return;

        ent.Comp.CategoriesPresent.Remove(category);
        foreach (var group in ent.Comp.OrganGroups.Values)
        {
            UpdateEffects(group, ent, ent.Comp.CategoriesPresent);
        }
    }

    #region private methods

    private void UpdateEffects(FunctionalOrgansGroup group,
        EntityUid ent,
        HashSet<ProtoId<OrganCategoryPrototype>> categories,
        bool initial = false)
    {
        var present = group.Categories.All(categories.Contains);

        if (present == group.Present && ! initial)
            return;

        group.Present = present;
        foreach (var effect in present? group.AppliedWhenAbsent : group.AppliedWhenPresent)
        {
            _statusEffects.TryRemoveStatusEffect(ent, effect);
        }

        foreach (var effect in present? group.AppliedWhenPresent : group.AppliedWhenAbsent)
        {
            _statusEffects.TryUpdateStatusEffectDuration(ent, effect);
        }
    }

    #endregion
}
