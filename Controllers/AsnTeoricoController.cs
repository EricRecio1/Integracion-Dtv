using IntegracionOcasaDtv.Models.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace IntegracionOcasaDtv.Controllers
{

    [ApiController]
    [Route("AsnTeorico")]
    public class AsnTeoricoController:ControllerBase
    {
        private readonly ILogger<AsnTeoricoController> _logger;
        private readonly AsnTeoricoService _asnTeoricoService;
        private readonly AsnReceptionService _asnReceptionService;
        private readonly TransferOrderService _transferOrderService;
        private readonly DespatchAdviceService _despatchAdviceService;
        private readonly ReceiptAdviceService _receiptAdviceService;
        private readonly SalesOrderService _salesOrderService;
        private readonly KittingWorkOrderService _kittingWorkOrderService;
        private readonly ReturnOrderService _returnOrderService;
        private readonly InventoryAdjustmentService _inventoryAdjustmentService;
        private readonly StockOcasaService _stockOcasaService;
        private readonly BuscadorErroresService _buscadorErroresService;
        private readonly GuardarArchivoService _guardarArchivoService;

        public AsnTeoricoController(ILogger<AsnTeoricoController> logger, IntegracionDtvContext context, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _asnTeoricoService = new AsnTeoricoService(context, configuration, logger);
            _asnReceptionService = new AsnReceptionService(context, configuration);
            _transferOrderService = new TransferOrderService(context, configuration);
            _despatchAdviceService = new DespatchAdviceService(context, configuration);
            _receiptAdviceService = new ReceiptAdviceService(context, configuration);
            _salesOrderService = new SalesOrderService(context, configuration);
            _kittingWorkOrderService = new KittingWorkOrderService(context, configuration);
            _returnOrderService = new ReturnOrderService(context, configuration);
            _inventoryAdjustmentService = new InventoryAdjustmentService(configuration, context);
            _stockOcasaService = new StockOcasaService(configuration);
            _buscadorErroresService = new BuscadorErroresService(context, configuration);
            _guardarArchivoService = new GuardarArchivoService(configuration, hostingEnvironment);
        }
        [Route("AsnTeorico")]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            try
            {
                _asnTeoricoService.ProcessFiles();
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError(e.InnerException.Message);
                    return BadRequest(e.InnerException.Message);
                }
                else if (!string.IsNullOrEmpty(e.Message))
                {
                    _logger.LogError(e.Message);
                    return BadRequest(e.Message);
                }
                else
                {
                    _logger.LogError("Error no capturado");
                    return BadRequest("Error no capturado");
                }
            }
        }

        [Route("AsnRecepcion")]
        [HttpGet]
        public async Task<ActionResult> GetRecepcion()
        {
            try
            {
                _asnReceptionService.GenerateReception();
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError(e.InnerException.Message);
                    return BadRequest(e.InnerException.Message);
                }
                else if (!string.IsNullOrEmpty(e.Message))
                {
                    _logger.LogError(e.Message);
                    return BadRequest(e.Message);
                }
                else
                {
                    _logger.LogError("Error no capturado");
                    return BadRequest("Error no capturado");
                }
            }
        }

        [Route("TransferOrder")]
        [HttpGet]
        public async Task<ActionResult> ProcessTransferOrder()
        {
            try
            {
                _transferOrderService.ProcessFiles();
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError(e.InnerException.Message);
                    return BadRequest(e.InnerException.Message);
                }
                else if (!string.IsNullOrEmpty(e.Message))
                {
                    _logger.LogError(e.Message);
                    return BadRequest(e.Message);
                }
                else
                {
                    _logger.LogError("Error no capturado");
                    return BadRequest("Error no capturado");
                }
            }
        }

        [Route("TransferOrderDespatch")]
        [HttpGet]
        public async Task<ActionResult> ProcessTransferOrderDespatch()
        {
            try
            {
                _despatchAdviceService.GenerateReception();
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError(e.InnerException.Message);
                    return BadRequest(e.InnerException.Message);
                }
                else if (!string.IsNullOrEmpty(e.Message))
                {
                    _logger.LogError(e.Message);
                    return BadRequest(e.Message);
                }
                else
                {
                    _logger.LogError("Error no capturado");
                    return BadRequest("Error no capturado");
                }
            }
        }

        [Route("TransferOrderReceipt")]
        [HttpGet]
        public async Task<ActionResult> ProcessTransferOrderReceipt()
        {
            try
            {
                _receiptAdviceService.GenerateReception();
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError(e.InnerException.Message);
                    return BadRequest(e.InnerException.Message);
                }
                else if (!string.IsNullOrEmpty(e.Message))
                {
                    _logger.LogError(e.Message);
                    return BadRequest(e.Message);
                }
                else
                {
                    _logger.LogError("Error no capturado");
                    return BadRequest("Error no capturado");
                }
            }
        }

        [Route("KittingWorkOrder")]
        [HttpGet]
        public async Task<ActionResult> ProcessKittingWorkOrder()
        {
            try
            {
                _kittingWorkOrderService.ProcessFiles();
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError(e.InnerException.Message);
                    return BadRequest(e.InnerException.Message);
                }
                else if (!string.IsNullOrEmpty(e.Message))
                {
                    _logger.LogError(e.Message);
                    return BadRequest(e.Message);
                }
                else
                {
                    _logger.LogError("Error no capturado");
                    return BadRequest("Error no capturado");
                }
            }

        }

        [Route("SalesOrder")]
        [HttpGet]
        public async Task<ActionResult> ProcessSalesOrder()
        {
            try
            {
                _salesOrderService.ProcessFiles();
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError(e.InnerException.Message);
                    return BadRequest(e.InnerException.Message);
                }
                else if (!string.IsNullOrEmpty(e.Message))
                {
                    _logger.LogError(e.Message);
                    return BadRequest(e.Message);
                }
                else
                {
                    _logger.LogError("Error no capturado");
                    return BadRequest("Error no capturado");
                }
            }
        }

        [Route("ReturnOrder")]
        [HttpGet]
        public async Task<ActionResult> ReturnOrder()
        {
            try
            {
                _returnOrderService.ProcessFiles();
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError(e.InnerException.Message);
                    return BadRequest(e.InnerException.Message);
                }
                else if (!string.IsNullOrEmpty(e.Message))
                {
                    _logger.LogError(e.Message);
                    return BadRequest(e.Message);
                }
                else
                {
                    _logger.LogError("Error no capturado");
                    return BadRequest("Error no capturado");
                }
            }

        }

        [Route("InventoryAdjustment")]
        [HttpGet]
        public async Task<ActionResult> InventoryAdjustment()
        {
            try
            {
                _inventoryAdjustmentService.GenerateInventoryAdjustment();
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError(e.InnerException.Message);
                    return BadRequest(e.InnerException.Message);
                }
                else if (!string.IsNullOrEmpty(e.Message))
                {
                    _logger.LogError(e.Message);
                    return BadRequest(e.Message);
                }
                else
                {
                    _logger.LogError("Error no capturado");
                    return BadRequest("Error no capturado");
                }
            }
        }

        [Route("StockOcasa")]
        [HttpGet]
        public async Task<ActionResult> StockOcasa()
        {
            try
            {
                _stockOcasaService.ProcessFiles();
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError(e.InnerException.Message);
                    return BadRequest(e.InnerException.Message);
                }
                if (!string.IsNullOrEmpty(e.Message))
                {
                    _logger.LogError(e.Message);
                    return BadRequest(e.Message);
                }
                _logger.LogError("Error no capturado");
                return BadRequest("Error no capturado");
            }
        }
        [Route("BuscadorErrores")]
        [HttpGet]
        public async Task<ActionResult> BuscadorErrores()
        {
            try
            {
                _buscadorErroresService.SaveFiles();
                return Ok();
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError(e.InnerException.Message);
                    return BadRequest(e.InnerException.Message);
                }
                if (!string.IsNullOrEmpty(e.Message))
                {
                    _logger.LogError(e.Message);
                    return BadRequest(e.Message);
                }
                _logger.LogError("Error no capturado");
                return BadRequest("Error no capturado");
            }
        }
        [Route("GuardarArchivo")]
        [HttpPost]
        public IActionResult GuardarArchivo([FromBody] string nombre_documento)
        {
            try
            {
                string tipoDoc = nombre_documento.Split(".")[^1];
                string nombreDocuAGuardar = nombre_documento.Replace(":", "");
                byte[] result = _guardarArchivoService.FindFile(nombre_documento);
                return new FileContentResult(result, "application/octet-stream");
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    _logger.LogError(e.InnerException.Message);
                    throw;
                }
                if (!string.IsNullOrEmpty(e.Message))
                {
                    _logger.LogError(e.Message);
                    throw;
                }
                _logger.LogError("Error no capturado");
                throw;
            }
        }
    }
}
