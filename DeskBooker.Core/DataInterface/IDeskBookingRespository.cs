using DeskBooker.Core.Domain;

namespace DeskBooker.Core.DataInterface
{
    public interface IDeskBookingRespository
    {
        void Save(DeskBooking deskBooking);
    }
}
