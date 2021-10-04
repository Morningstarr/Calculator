using CalculatorService.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorService.Services
{
    public class CalculatorService : Calctor.CalctorBase
    {
        private readonly ILogger<CalculatorService> _logger;

        private Calculator calculator;

        public CalculatorService(ILogger<CalculatorService> logger)
        {
            _logger = logger;

            calculator = new Calculator();

        }

        public override Task<Result> Summa(Arguments request, ServerCallContext context)
        {
            var result = calculator.Sum(request.First, request.Second);

            return Task.FromResult(new Result { Res = result });
        }

        public override Task<Result> Division(Arguments request, ServerCallContext context)
        {
            var result = calculator.Division(request.First, request.Second);

            return Task.FromResult(new Result { Res = result });
        }

        public override Task<Result> Difference(Arguments request, ServerCallContext context)
        {
            var result = calculator.Difference(request.First, request.Second);

            return Task.FromResult(new Result { Res = result });
        }

        public override Task<Result> Mult(Arguments request, ServerCallContext context)
        {
            var result = calculator.Multiplication(request.First, request.Second);

            return Task.FromResult(new Result { Res = result });
        }
    }
}
