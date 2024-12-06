from typing import List, Set, Tuple

def turn_right(direction: str) -> str:
    """
    Turn the guard's facing direction 90 degrees to the right.

    Parameters:
        direction (str): One of '^', '>', 'v', '<' representing up, right, down, left.

    Returns:
        str: The new direction after turning right.
    """
    if direction == '^':
        return '>'
    elif direction == '>':
        return 'v'
    elif direction == 'v':
        return '<'
    elif direction == '<':
        return '^'
    # Fallback shouldn't happen if input is correct
    return direction

def forward_pos(x: int, y: int, direction: str) -> Tuple[int, int]:
    """
    Given a position and a direction, return the position one step forward.

    Parameters:
        x (int): Current row.
        y (int): Current column.
        direction (str): Current direction '^', '>', 'v', or '<'.

    Returns:
        Tuple[int,int]: The position one step forward.
    """
    if direction == '^':
        return x-1, y
    elif direction == 'v':
        return x+1, y
    elif direction == '<':
        return x, y-1
    elif direction == '>':
        return x, y+1
    return x, y

def simulate_patrol(grid: List[List[str]], start_x: int, start_y: int, start_dir: str) -> Tuple[Set[Tuple[int,int]], bool]:
    """
    Simulate the guard's patrol until leaving the map.

    Rules:
    - If there is something (#) directly in front, turn right.
    - Otherwise, move forward.

    Parameters:
        grid (List[List[str]]): The map of the lab.
        start_x (int): Starting row of the guard.
        start_y (int): Starting column of the guard.
        start_dir (str): Starting direction of the guard.

    Returns:
        Tuple[Set[Tuple[int,int]], bool]: A set of visited positions and a boolean indicating if the guard left the map.
    """
    rows = len(grid)
    cols = len(grid[0])
    direction = start_dir
    x, y = start_x, start_y

    visited_positions = {(x, y)}

    while True:
        fx, fy = forward_pos(x, y, direction)
        # Check if out of bounds
        if fx < 0 or fx >= rows or fy < 0 or fy >= cols:
            # Guard leaves the map
            return visited_positions, True
        # Check if blocked
        if grid[fx][fy] == '#':
            # Turn right
            direction = turn_right(direction)
        else:
            x, y = fx, fy
            visited_positions.add((x, y))

def simulate_patrol_with_loop_check(grid: List[List[str]], start_x: int, start_y: int, start_dir: str) -> bool:
    """
    Simulate the guard's patrol and check if the guard gets stuck in a loop.

    A loop occurs if the guard repeats the same state (position and direction) more than once.
    If the guard leaves the map, return True (no loop).
    If a loop is detected, return False.

    Parameters:
        grid (List[List[str]]): The map of the lab.
        start_x (int): Starting row.
        start_y (int): Starting column.
        start_dir (str): Starting direction.

    Returns:
        bool: True if the guard leaves the map, False if stuck in a loop.
    """
    rows = len(grid)
    cols = len(grid[0])
    direction = start_dir
    x, y = start_x, start_y

    visited_states = {(x, y, direction)}

    while True:
        fx, fy = forward_pos(x, y, direction)
        # Check if out of bounds
        if fx < 0 or fx >= rows or fy < 0 or fy >= cols:
            return True  # left the map, no loop

        if grid[fx][fy] == '#':
            direction = turn_right(direction)
        else:
            x, y = fx, fy

        state = (x, y, direction)
        if state in visited_states:
            return False  # loop detected
        visited_states.add(state)

def main():
    """Execute Day 6 puzzle solution."""
    with open("../data/Day6.txt", "r") as f:
        grid = [list(line.rstrip('\n')) for line in f]

    rows = len(grid)
    cols = len(grid[0]) if rows > 0 else 0

    # Find guard starting position and direction
    directions = ['^','>','v','<']
    start_x, start_y, start_dir = None, None, None
    for i in range(rows):
        for j in range(cols):
            if grid[i][j] in directions:
                start_x, start_y = i, j
                start_dir = grid[i][j]
                break
        if start_dir is not None:
            break

    # Replace the guard symbol with '.' for uniformity
    grid[start_x][start_y] = '.'

    # Part One
    visited_positions, _ = simulate_patrol(grid, start_x, start_y, start_dir)
    part_one_result = len(visited_positions)
    print("Day 6 - Part 1 Result:", part_one_result)

    # Part Two
    # Test placing a new obstruction at each '.' position (except the start)
    loop_count = 0
    for i in range(rows):
        for j in range(cols):
            # Skip starting position and only consider '.'
            if (i, j) == (start_x, start_y):
                continue
            if grid[i][j] != '.':
                continue

            # Temporarily place obstruction
            grid[i][j] = '#'
            left_map = simulate_patrol_with_loop_check(grid, start_x, start_y, start_dir)
            if not left_map:
                loop_count += 1

            # Restore cell
            grid[i][j] = '.'

    print("Day 6 - Part 2 Result:", loop_count)

if __name__ == "__main__":
    main()
