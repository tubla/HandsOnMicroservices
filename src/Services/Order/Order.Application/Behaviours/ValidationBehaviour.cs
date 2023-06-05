using FluentValidation;
using MediatR;
using ValidationException = Order.Application.Exceptions.ValidationException;

namespace Order.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
               var validationContext = new ValidationContext<TRequest>(request);
               var validationResult = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(validationContext, cancellationToken)));
               var failures = validationResult.SelectMany(r => r.Errors).Where(f => f != null).ToList();
                if(failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }
            return await next();
        }
    }
}
