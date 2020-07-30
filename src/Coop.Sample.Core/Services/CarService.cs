using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coop.Sample.Core.Dtos;

namespace Coop.Sample.Core.Services
{
    public interface ICarService
    {
        Task<IEnumerable<CarDto>> GetAllSortedByPlateAsync(CancellationToken cancellationToken);
    }

    public class CarService : ICarService
    {
        public Task<IEnumerable<CarDto>> GetAllSortedByPlateAsync(CancellationToken cancellationToken)
        {
            // Mock
            var list = new List<CarDto>() {
                new CarDto() {
                    Id = 123,
                    Plate = "ABC-123",
                    Model = "Honda Accord"
                }
            };

            IEnumerable<CarDto> data = list;
            return Task.Delay(1).ContinueWith(t => data);
        }
    }
}
