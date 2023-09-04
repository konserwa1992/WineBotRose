using CodeInject.Actors;

namespace CodeInject.PickupFilters
{
    public interface IFilter
    {
        bool CanPickup(IObject item);
    }
}
