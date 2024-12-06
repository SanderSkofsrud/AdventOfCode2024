from typing import List

def count_word_occurrences(grid: List[str], word: str) -> int:
    """
    Count the number of occurrences of a given word in all eight directions of a 2D grid.
    Directions: horizontal, vertical, and both diagonals (forward and backward).

    Parameters:
        grid (List[str]): The 2D grid represented as a list of strings.
        word (str): The word to search for.

    Returns:
        int: The total number of occurrences of the word in the grid.
    """
    rows, cols = len(grid), len(grid[0])
    word_length = len(word)
    total_count = 0

    directions = [
        (0, 1), (0, -1),
        (1, 0), (-1, 0),
        (1, 1), (-1, -1),
        (1, -1), (-1, 1),
    ]

    def is_valid(r: int, c: int) -> bool:
        return 0 <= r < rows and 0 <= c < cols

    def count_from_position(r: int, c: int, direction: tuple) -> int:
        for i in range(word_length):
            nr, nc = r + i * direction[0], c + i * direction[1]
            if not is_valid(nr, nc) or grid[nr][nc] != word[i]:
                return 0
        return 1

    for r in range(rows):
        for c in range(cols):
            for direction in directions:
                total_count += count_from_position(r, c, direction)

    return total_count

def count_xmas_pattern(grid: List[str]) -> int:
    """
    Count occurrences of a specific X-MAS pattern around the letter 'A'.
    Patterns are defined by certain arrangements of 'X', 'M', 'A', 'S' around 'A'.

    Parameters:
        grid (List[str]): The 2D grid represented as a list of strings.

    Returns:
        int: The total number of occurrences of the X-MAS pattern.
    """
    rows, cols = len(grid), len(grid[0])
    total_count = 0

    def is_valid(r: int, c: int) -> bool:
        return 0 <= r < rows and 0 <= c < cols

    def check_xmas_pattern(center_row: int, center_col: int) -> int:
        if not is_valid(center_row, center_col) or grid[center_row][center_col] != "A":
            return 0

        # Each pattern is a list of ((dr, dc), char) tuples
        patterns = [
            [((-1, -1), 'M'), ((-1, 1), 'M'), ((1, -1), 'S'), ((1, 1), 'S')],
            [((-1, -1), 'S'), ((-1, 1), 'S'), ((1, -1), 'M'), ((1, 1), 'M')],
            [((-1, -1), 'S'), ((-1, 1), 'M'), ((1, -1), 'S'), ((1, 1), 'M')],
            [((-1, -1), 'M'), ((-1, 1), 'S'), ((1, -1), 'M'), ((1, 1), 'S')]
        ]

        valid_patterns = 0
        for pattern in patterns:
            if all(is_valid(center_row + dr, center_col + dc)
                   and grid[center_row + dr][center_col + dc] == char
                   for (dr, dc), char in pattern):
                valid_patterns += 1
        return valid_patterns

    for r in range(rows):
        for c in range(cols):
            total_count += check_xmas_pattern(r, c)

    return total_count

def read_grid_from_file(filename: str) -> List[str]:
    """
    Read the puzzle input as a list of strings, each string representing a row of the grid.

    Parameters:
        filename (str): The path to the input file.

    Returns:
        List[str]: The grid as a list of strings.
    """
    with open(filename, 'r') as file:
        return [line.strip() for line in file]

def main():
    """Execute Day 4 puzzle solution."""
    file_path = '../data/Day4.txt'
    grid = read_grid_from_file(file_path)

    # Part 1
    word = "XMAS"
    part1_result = count_word_occurrences(grid, word)
    print(f"Day 4 - Part 1: Total occurrences of '{word}': {part1_result}")

    # Part 2
    part2_result = count_xmas_pattern(grid)
    print(f"Day 4 - Part 2: Total occurrences of X-MAS pattern: {part2_result}")

if __name__ == "__main__":
    main()
