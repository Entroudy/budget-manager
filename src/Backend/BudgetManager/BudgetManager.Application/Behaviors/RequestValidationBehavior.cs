namespace BudgetManager.Application.Behaviors
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Validation error occurred executing: {request.GetType().ToString()}");

            if (failures.Any())
            {
                failures.ForEach(x => stringBuilder.AppendLine($"Property \"{x.PropertyName}\" is invalid, value: \"{x.AttemptedValue}\" - ErrorMessage: {x.ErrorMessage}"));
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}