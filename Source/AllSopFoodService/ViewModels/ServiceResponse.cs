namespace AllSopFoodService.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ServiceResponse<T>
    {
        public T Data { get; set; } = default!;
        public bool Success { get; set; } = true;
        public string Message { get; set; } = default!;
    }
}
