using System.Collections.Generic;
using Entity;
using TouchInput.UnitControl;
using UnityEngine;

namespace Interactions
{
    public struct TargetingInfo : ITargetingInfo<ITargetable, ITargetable>
    {
        private readonly ITargetable _source;
        private readonly ITargetable _target;

        public ITargetable Source => _source;
        public ITargetable Target => _target;

        public TargetingInfo(ITargetable source, ITargetable target)
        {
            _source = source;
            _target = target;
        }

        public bool TryGetTargetEntity<TEntity>(out TEntity entity)
        {
            if (_target != null)
            {
                if (_target is IEntityView<TEntity> entityView)
                {
                    entity = entityView.Entity;
                    return true;
                }
                if (_target.TryGetGameObject(out var go))
                {
                    var entityViewComponent = go.GetComponent<IEntityView<TEntity>>();
                    if (entityViewComponent != null)
                    {
                        entity = entityViewComponent.Entity;
                        return true;
                    }
                }
            }
            entity = default;
            return false;
        }
        
        public bool TryGetEntitiesInRange<TEntity>(out HashSet<TEntity> targetEntities, float range)
        {
            GetTargetsInRange(range, out var targets);
            targetEntities = new HashSet<TEntity>();
            foreach (var target in targets)
            {
                /*
                if (target is IEntityView<TEntity> entityView)
                {
                    if (entityView.Entity != null)
                    {
                        targetEntities.Add(entityView.Entity);
                    }
                   
                    return true;
                }*/
                if (target.TryGetGameObject(out var go))
                {
                    var entityViewComponent = go.GetComponent<IEntityView<TEntity>>();
                    if (entityViewComponent != null)
                    {
                        if (entityViewComponent.Entity != null)
                        {
                            targetEntities.Add(entityViewComponent.Entity);
                        }
                    }
                }
            }
            return targetEntities.Count > 0;
        }

        //TODO - this needs to hit all targets and not just UnitInputHandler
        public static bool GetTargetsInRange(ITargetable target, float radius, out List<UnitInputHandler> targets)
        {
            Vector3 targetPosition = target.GetInteractionPosition();
            return UnitInputController.GetTargetsInRadius(targetPosition, radius, out targets);
        }
        
        public bool GetTargetsInRange(float radius, out List<UnitInputHandler> targets)
        {
            return GetTargetsInRange(_target, radius, out targets);
        }

        public float GetDistance()
        {
            if (_source != null && _target != null)
            {
                Vector3 sourcePos = _source.GetInteractionPosition();
                Vector3 targetPos = _target.GetInteractionPosition();
                return Vector3.Distance(sourcePos, targetPos);
            }
            return 0;
        }

        public Vector3 DirectionToTarget()
        {
            if (_target != null)
            {
                return (_source.GetInteractionPosition() - _target.GetInteractionPosition()).normalized;
            }
            return Vector3.zero;
        }
    }
    
    public interface ITargetingInfo <TSource, TTarget> where TSource : ITargetable where TTarget : ITargetable
    {
        public TSource Source { get; }
        public TTarget Target { get; }
    }
}