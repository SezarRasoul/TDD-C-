using System;
using System.Collections.Generic;
using System.Linq;
using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Domain;
using Moq;
using Xunit;

namespace DeskBooker.Core.Processor
{
    public class DeskBookingRequestProcessotTests
    {
        private readonly DeskBookingRequest _request;
        private readonly List<Desk> _availableDesks;
        private readonly Mock<IDeskBookingRespository> _deskBookingRespository;
        private readonly Mock<IDeskRespository> _deskRespository;
        private readonly DeskBookingRequestProcessor _processor;

        public DeskBookingRequestProcessotTests()
        {
            _request = new DeskBookingRequest
            {
                FirstName = "Sezar",
                LastName = "Rasoul",
                Email = "sezar@example.com",
                Date = new DateTime(2021, 05, 17)
            };

            _availableDesks = new List<Desk> { new Desk { Id = 7 } };

            _deskBookingRespository = new Mock<IDeskBookingRespository>();
            _deskRespository = new Mock<IDeskRespository>();
            _deskRespository.Setup(x => x.GetAvailableDesks(_request.Date))
                .Returns(_availableDesks);

            _processor = new DeskBookingRequestProcessor(
                _deskBookingRespository.Object, _deskRespository.Object);
        }

        [Fact]
        public void ShouldReturnDeskBookingRequestWithRequestValues()
        {  
            //Act
            DeskBookingResult result = _processor.BookDesk(_request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_request.FirstName, result.FirstName);
            Assert.Equal(_request.LastName, result.LastName);
            Assert.Equal(_request.Email, _request.Email);
            Assert.Equal(_request.Date, result.Date);
        }

        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));

            Assert.Equal("request", exception.ParamName);
        }

        [Fact]
        public void ShouldSaveDeskBooking()
        {
            DeskBooking savedDeskBooking = null;
            _deskBookingRespository.Setup(x => x.Save(It.IsAny<DeskBooking>()))
                .Callback<DeskBooking>(deskBooking =>
                {
                    savedDeskBooking = deskBooking;
                });

            _processor.BookDesk(_request);

            _deskBookingRespository.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Once);

            Assert.NotNull(savedDeskBooking);
            Assert.Equal(_request.FirstName, savedDeskBooking.FirstName);
            Assert.Equal(_request.LastName, savedDeskBooking.LastName);
            Assert.Equal(_request.Email, savedDeskBooking.Email);
            Assert.Equal(_request.Date, savedDeskBooking.Date);
            Assert.Equal(_availableDesks.First().Id, savedDeskBooking.DeskId);
        }
        [Fact]
        public void ShouldNotSaveDeskBookingIfNoDeskIsAvailable()
        {
            // TODO: Ensure that no desk is available
            _availableDesks.Clear();

            _processor.BookDesk(_request);

            _deskBookingRespository.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Never);
        }
    }
}
