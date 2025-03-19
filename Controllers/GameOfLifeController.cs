using GameOfLife_A.Models;
using GameOfLife_A.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace GameOfLife_A.Controllers;

/// <summary>
/// Controller for managing the Game of Life operations.
/// </summary>
[ApiController]
[Route("[controller]")]
public class GameOfLifeController : ControllerBase
{
   private readonly GameOfLifeContext _dbContext;
   private readonly ILogger<GameOfLifeController> _logger;
   private readonly GameOfLifeService _service = new GameOfLifeService();

   /// <summary>
   /// Initializes a new instance of the <see cref="GameOfLifeController"/> class.
   /// </summary>
   /// <param name="logger">The logger for logging controller events.</param>
   /// <param name="context">The database context used to store game state.</param>
   /// <param name="service">The Game of Life service that contains business logic.</param>
   public GameOfLifeController(ILogger<GameOfLifeController> logger, GameOfLifeContext context,
      GameOfLifeService service)
   {
      _logger = logger;
      _dbContext = context;
      _service = service;
   }

   /// <summary>
   /// Uploads a new board state.
   /// </summary>
   /// <param name="arrayInText">The board state represented as string 
   /// that uses commas to divide the values and semicolons to divide rows
   /// <returns>The ID of the newly created board if successful; otherwise, a bad request or error message.</returns>
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

   /// <summary>
   /// Retrieves all saved boards.
   /// </summary>
   /// <returns>A list of saved boards; otherwise, a not found result.</returns>
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

   /// <summary>
   /// Retrieves the next state for a given board.
   /// </summary>
   /// <param name="id">The ID of the board.</param>
   /// <returns>The next state of the board; otherwise, a not found result.</returns>
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
      //Iterations needed to calculate the next state
      const int numOfIterations = 1;
      var result = _service.XAwayState(board.Array, numOfIterations);

      return Ok(result);
   }

   /// <summary>
   /// Retrieves the state of a board after a specified number of state transitions.
   /// </summary>
   /// <param name="id">The ID of the board.</param>
   /// <param name="numberOfStatesAway">The number of state transitions to apply.</param>
   /// <returns>The board state after the specified number of transitions; otherwise, a not found result.</returns>
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

   /// <summary>
   /// Retrieves the final state of a board after a specified number of attempts.
   /// </summary>
   /// <param name="id">The ID of the board.</param>
   /// <param name="numberOfAttempts">The maximum number of state transitions to try before considering the board stable.</param>
   /// <returns>The final stable state if found; otherwise, an error message.</returns>
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
         return StatusCode(int.Parse(HttpStatusCode.InternalServerError.ToString()), 
            $"Final state not found after {numberOfAttemps} attemps");
      }
      else
      {
         return Ok(result);
      }

   }
}
