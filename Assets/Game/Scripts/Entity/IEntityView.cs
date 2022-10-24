namespace Entity
{
    public interface IEntityView<TEntity>
    {
        public TEntity Entity { get; }
    }
}