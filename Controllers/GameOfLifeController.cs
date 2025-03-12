using GameOfLife_A.Models;
using GameOfLife_A.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameOfLife_A.Controllers;

[ApiController]
[Route("[controller]")]
public class GameOfLifeController : ControllerBase
{
   private readonly GameOfLifeContext _dbContext;
   private readonly ILogger<GameOfLifeController> _logger;
   private readonly GameOfLifeService _service = new GameOfLifeService();

   public GameOfLifeController(ILogger<GameOfLifeController> logger, GameOfLifeContext context,
      GameOfLifeService service)
   {
      _logger = logger;
      _dbContext = context;
      _service = service;
   }

   //1. Upload new board state
   [HttpPost("upload/")]
   public async Task<ActionResult> Post(string arrayInText)
   {
      Board newBoard = new()
      {
         InternalArray = arrayInText
      };

      try
      {
         int[][] boardArray = newBoard.Array;
      }
      catch (Exception)
      {
         return BadRequest("The string could not be converted into a valid array. Use commas to divide values and semi colons to divide rows");
      }
      if (_dbContext.Boards == null)
      {
         _logger.LogError("_dbContext.Boards is null.");
         return Problem("Entity set 'DbContext.Boards' is null.");
      }

      _dbContext.Boards.Add(newBoard);
      await _dbContext.SaveChangesAsync();

      return Ok(newBoard.Id);
   }

   [HttpGet("boards/")]
   public async Task<ActionResult<IEnumerable<Board>>> GetBoards()
   {
      if (_dbContext.Boards == null)
      {
         _logger.LogError("_dbContext.Boards is null.");
         return NotFound("Entity set 'DbContext.Boards' is null.");
      }
      return await _dbContext.Boards.ToListAsync();
   }

   //2. Gets next state for board, returns next state
   [HttpGet("nextState/")]
   public async Task<IActionResult> Get(int id)
   {
      if (_dbContext.Boards == null)
      {
         _logger.LogError("_dbContext.Boards is null.");
         return NotFound();
      }
      var board = await _dbContext.Boards.FindAsync(id);
      if (board == null)
      {
         return NotFound("No board exists with the id sent");
      }

      var result = _service.XAwayState(board.Array, 1);

      return Ok(result);
   }

   //3. Gets x number of states away for board
   [HttpGet("statesAway/")]
   public async Task<IActionResult> Get(int id, int numberOfStatesAway)
   {
      if (_dbContext.Boards == null)
      {
         _logger.LogError("_dbContext.Boards is null.");
         return NotFound();
      }
      var board = await _dbContext.Boards.FindAsync(id);
      if (board == null)
      {
         return NotFound("No board exists with the id sent");
      }

      var result = _service.XAwayState(board.Array, numberOfStatesAway);

      return Ok(result);
   }

   //4. Gets final state for board
   [HttpGet("finalState/")]
   public async Task<IActionResult> GetFinalState(int id, int numberOfAttemps)
   {
      if (_dbContext.Boards == null)
      {
         _logger.LogError("_dbContext.Boards is null.");
         return NotFound();
      }
      var board = await _dbContext.Boards.FindAsync(id);
      if (board == null)
      {
         return NotFound("No board exists with the id sent");
      }

      var result = _service.FinalState(board.Array, numberOfAttemps);
      if (result == null)
      {
         return StatusCode(500, $"Final state not found after {numberOfAttemps} attemps");
      }
      else
      {
         return Ok(result);
      }

   }
}
