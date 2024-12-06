from collections import Counter
from typing import List, Tuple

def calculate_total_distance(left_list: List[int], right_list: List[int]) -> int:
    """
    Calculate the total distance between corresponding elements of two integer lists.
    Both lists are first sorted to pair smallest with smallest, etc.
    The distance is the sum of absolute differences of paired elements.

    Parameters:
        left_list (List[int]): The first list of integers.
        right_list (List[int]): The second list of integers.

    Returns:
        int: The sum of absolute differences between paired elements.
    """
    left_sorted = sorted(left_list)
    right_sorted = sorted(right_list)
    return sum(abs(l - r) for l, r in zip(left_sorted, right_sorted))

def calculate_similarity_score(left_list: List[int], right_list: List[int]) -> int:
    """
    Calculate a similarity score between two lists.
    The score is defined as the sum of the product of an element in `left_list` and
    its occurrences in `right_list`.

    Parameters:
        left_list (List[int]): The first list of integers.
        right_list (List[int]): The second list of integers.

    Returns:
        int: The calculated similarity score.
    """
    right_count = Counter(right_list)
    return sum(num * right_count[num] for num in left_list)

def read_lists_from_file(filename: str) -> Tuple[List[int], List[int]]:
    """
    Read pairs of integers from the given file and separate them into two lists.

    The file is expected to have lines with two integers per line: `left right`.

    Parameters:
        filename (str): The path to the input file.

    Returns:
        Tuple[List[int], List[int]]: Two lists, one containing all left integers and the other all right integers.
    """
    left_list, right_list = [], []
    with open(filename, 'r') as file:
        for line in file:
            left, right = map(int, line.split())
            left_list.append(left)
            right_list.append(right)
    return left_list, right_list

def main():
    """Execute Day 1 puzzle solution."""
    file_path = '../data/Day1.txt'
    left_list, right_list = read_lists_from_file(file_path)

    # Part 1
    total_distance = calculate_total_distance(left_list, right_list)
    print(f"Day 1 - Part 1: Total distance: {total_distance}")

    # Part 2
    similarity_score = calculate_similarity_score(left_list, right_list)
    print(f"Day 1 - Part 2: Similarity score: {similarity_score}")

if __name__ == "__main__":
    main()
