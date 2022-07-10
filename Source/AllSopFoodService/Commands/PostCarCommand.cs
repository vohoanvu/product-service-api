namespace AllSopFoodService.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Constants;
    using ViewModels;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;
    using Repositories.Interfaces;

    public class PostCarCommand
    {
        private readonly ICarRepository carRepository;
        private readonly IMapper<Model.Car, Car> carToCarMapper;
        private readonly IMapper<SaveCar, Model.Car> saveCarToCarMapper;

        public PostCarCommand(
            ICarRepository carRepository,
            IMapper<Model.Car, Car> carToCarMapper,
            IMapper<SaveCar, Model.Car> saveCarToCarMapper)
        {
            this.carRepository = carRepository;
            this.carToCarMapper = carToCarMapper;
            this.saveCarToCarMapper = saveCarToCarMapper;
        }

        public async Task<IActionResult> ExecuteAsync(SaveCar saveCar, CancellationToken cancellationToken)
        {
            var car = this.saveCarToCarMapper.Map(saveCar);
            car = await this.carRepository.AddAsync(car, cancellationToken).ConfigureAwait(false);
            var carViewModel = this.carToCarMapper.Map(car);

            return new CreatedAtRouteResult(
                CarsControllerRoute.GetCar,
                new { carId = carViewModel.CarId },
                carViewModel);
        }
    }
}
