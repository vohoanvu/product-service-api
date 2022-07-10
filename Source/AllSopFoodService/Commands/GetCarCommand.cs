namespace AllSopFoodService.Commands
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using ViewModels;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Net.Http.Headers;
    using Repositories.Interfaces;

    public class GetCarCommand
    {
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly ICarRepository carRepository;
        private readonly IMapper<Model.Car, Car> carMapper;

        public GetCarCommand(
            IActionContextAccessor actionContextAccessor,
            ICarRepository carRepository,
            IMapper<Model.Car, Car> carMapper)
        {
            this.actionContextAccessor = actionContextAccessor;
            this.carRepository = carRepository;
            this.carMapper = carMapper;
        }

        public async Task<IActionResult> ExecuteAsync(int carId, CancellationToken cancellationToken)
        {
            var car = await this.carRepository.GetAsync(carId, cancellationToken).ConfigureAwait(false);
            if (car is null)
            {
                return new NotFoundResult();
            }

            var actionContext = this.actionContextAccessor.ActionContext;
            if (actionContext != null)
            {
                var httpContext = actionContext.HttpContext;
                if (httpContext.Request.Headers.TryGetValue(HeaderNames.IfModifiedSince, out var stringValues))
                {
                    if (DateTimeOffset.TryParse(stringValues, out var modifiedSince) &&
                        (modifiedSince >= car.Modified))
                    {
                        return new StatusCodeResult(StatusCodes.Status304NotModified);
                    }
                }

                var carViewModel = this.carMapper.Map(car);
                httpContext.Response.Headers.Add(
                    HeaderNames.LastModified,
                    car.Modified.ToString("R", CultureInfo.InvariantCulture));
                return new OkObjectResult(carViewModel);
            }

            return new NotFoundResult();
        }
    }
}
