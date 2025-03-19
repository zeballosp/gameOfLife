# Requirements

## 🛠️ **Install `dotnet-ef`**
1. Ensure you have the `dotnet-ef` tool installed.  
   - To install it, run:  
   ```bash
   dotnet tool install --global dotnet-ef
If you can't run the command, install dotnet-ef globally using:
   ```bash
   dotnet tool install --global dotnet-ef
```
## 🛠️ **Connection String**

1. Check the connection string in appsettings.json.
The database is configured for a local instance of an SQL Server, assuming a database named **GameOfLifeDB** exists. 
If you don't have that database you can create an empty database with that same name.
Confirm that the connection string is correct in appsettings.json.

# The project

This is an implementation of Connan's Game of Life. 

These are the rules followed:

1. Any live cell with fewer than two live neighbors dies(underpopulation).
2. Any live cell with more than three live neighbors dies(overpopulation).
3. Any live cell with two or three live neighbors survives to the next generation.
4. Dead cell becomes alive if it has exactly 3 neighbors

To count the neighboors the solution does NOT consider wrapping only counting the neighboors around if they are inside the array

# The project structure

The project contains the following folders for better separation of concerns: Controllers, Database, Models, Services and Tests.
The endpoints exposed at the moment are the following

### Upload new board state
`POST /GameOfLife/upload`

**Parameters:**
- **arrayInText** (string) - The array in text format to upload.

**Responses:**
- **200 OK**: Success

---

### Get all boards
`GET /GameOfLife/boards`

**Responses:**
- **200 OK**: Success (Returns a list of boards in JSON format)

---

### Get next state for board
`GET /GameOfLife/nextState`

**Parameters:**
- **id** (integer) - The ID of the board.

**Responses:**
- **200 OK**: Success

---

### Get x number of states away
`GET /GameOfLife/statesAway`

**Parameters:**
- **id** (integer) - The ID of the board.
- **numberOfStatesAway** (integer) - The number of states to move forward.

**Responses:**
- **200 OK**: Success

---

### Get final state for board
`GET /GameOfLife/finalState`

**Parameters:**
- **id** (integer) - The ID of the board.
- **numberOfAttemps** (integer) - The number of attempts to reach the final state.

**Responses:**
- **200 OK**: Success
---

## Models

### Board

**Properties:**
- **id** (integer) - The ID of the board.
- **array** (array of arrays of integers, nullable) - The 2D array representation of the board.

---

## Example Responses

**GET /GameOfLife/boards**
```json
[
  {
    "id": 1,
    "internalArray": "0,0,0;0,1,0;0,0,0",
    "array": [[0, 0, 0], [0, 1, 0], [0, 0, 0]],
    "arrayRows": 3,
    "arrayColumns": 3
  }
]
```

**GET /GameOfLife/nextstate**
```json
{
  "id": 1,
  "internalArray": "0,0,0;0,0,0;0,1,0",
  "array": [[0, 0, 0], [0, 0, 0], [0, 1, 0]],
  "arrayRows": 3,
  "arrayColumns": 3
}
```

**GET /GameOfLife/finalState**
```json
{
  "id": 1,
  "internalArray": "0,0,0;0,0,0;0,1,0",
  "array": [[0, 0, 0], [0, 0, 0], [0, 1, 0]],
  "arrayRows": 3,
  "arrayColumns": 3
}
```