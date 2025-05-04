using Business.Interfaces;
using Business.Models;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ClientsController(IClientService clientService) : Controller
    {

        private readonly IClientService _clientService = clientService;

        [HttpPost]
        public async Task<IActionResult> Add(AddClientForm form)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                    );

                return BadRequest(new { success = false, errors });
            }

            //send data to clientService
            var result = await _clientService.AddClientAsync(form);

            if (result)
            {
                return Ok(new { success = true });

            }else
            {
                return Problem("Unable to submit data");
    }


}


        [HttpPost]
        public IActionResult Edit(EditClientForm form)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                    );

                return BadRequest(new { success = false, errors });
            }

            // send data to clientService

            // send data to clientService
            //var result = await _clientService.EditClientAsync(form);
            //if(result)
            //{
            return Ok(new { success = true });

            //}else
            //{
            //    return Problem("Unable to submit data");
            //}
        }
    }
}
