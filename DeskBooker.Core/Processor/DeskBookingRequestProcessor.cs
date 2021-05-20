using System;
using System.Linq;
using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessor
    {
        private readonly IDeskBookingRespository _deskBookingRespository;
        private readonly IDeskRespository _deskRespository;

        public DeskBookingRequestProcessor(IDeskBookingRespository _deskBookingRespository, IDeskRespository _deskRespository)
        {
            this._deskBookingRespository = _deskBookingRespository;
            this._deskRespository = _deskRespository;
        }

        public DeskBookingResult BookDesk(DeskBookingRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var availableDesk = _deskRespository.GetAvailableDesks(request.Date);
            if (availableDesk.FirstOrDefault() is Desk availableDesks)
            {
                
                var deskBooking = Create<DeskBooking>(request);
                deskBooking.DeskId = availableDesks.Id;
                _deskBookingRespository.Save(deskBooking);

            }
            return Create<DeskBookingResult>(request);
        }

        private static T Create<T>(DeskBookingRequest request) where T : DeskBookingBase,new()
        {
            return new T
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Date = request.Date
            };
        }
    }
}