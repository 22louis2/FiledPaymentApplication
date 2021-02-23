using AutoMapper;
using FiledPaymentApplication.Common;
using FiledPaymentApplication.Core;
using FiledPaymentApplication.DTO;
using FiledPaymentApplication.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FiledPaymentApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly ICheapPaymentGateway _cheapPayment;
        private readonly IExpensivePaymentGateway _expensivePayment;
        private readonly IPremiumPaymentGateway _premiumPayment;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public PaymentController(ILogger<PaymentController> logger, IServiceProvider provider, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _cheapPayment = provider.GetRequiredService<ICheapPaymentGateway>();
            _expensivePayment = provider.GetRequiredService<IExpensivePaymentGateway>();
            _premiumPayment = provider.GetRequiredService<IPremiumPaymentGateway>();
            _transactionRepository = provider.GetRequiredService<ITransactionRepository>();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentToCreateDTO model)
        {
            model.CreditCardNumber = Regex.Replace(model.CreditCardNumber, @"[^\d]", "");
            var validator = new PaymentValidator();
            var validate = await validator.ValidateAsync(model);            

            if(!validate.IsValid)
            {
                var errors = validator.GetErrorMessage(validate.Errors);
                return BadRequest(new { message = "Bad request", errors });
            }             

            Payment payment;
            try
            {
                payment = _mapper.Map<Payment>(model);

                if (model.Amount < 21)
                    await _cheapPayment.ProcessPayment(payment);
                else if (model.Amount < 501)
                {
                    try
                    {
                        await _expensivePayment.ProcessPayment(payment);
                    }
                    catch (Exception)
                    {
                        await _cheapPayment.ProcessPayment(payment);
                    }
                }
                else
                {
                    try
                    {
                        await _premiumPayment.ProcessPayment(payment);
                    }
                    catch (Exception)
                    {
                        int i = 1;
                        do
                        {
                            var response = await _cheapPayment.ProcessPayment(payment);
                            if (response)
                                break;
                            i++;
                        }
                        while (i < 4);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { message = "Internal error", errors = "Data access processing error" });
            }

            try
            {
                var result = await _transactionRepository.GetByPaymentId(payment.Id);
                return Ok(new { message = "Success", data = result });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500, new { message = "Internal error", errors = "Data access processing error" });
            }          
        }
    }
}
