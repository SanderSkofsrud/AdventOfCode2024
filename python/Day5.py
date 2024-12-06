from collections import defaultdict, deque
from typing import List, Tuple

def parse_input(filename: str) -> Tuple[List[Tuple[int,int]], List[List[int]]]:
    """
    Parse the puzzle input file which contains ordering rules followed by updates.

    Parameters:
        filename (str): The path to the input file.

    Returns:
        Tuple[List[Tuple[int,int]], List[List[int]]]: A tuple containing the rules
        (as a list of (x, y) tuples) and updates (as a list of lists of page numbers).
    """
    with open(filename, "r") as f:
        lines = [line.strip() for line in f]

    blank_index = lines.index('')
    rule_lines = lines[:blank_index]
    update_lines = lines[blank_index+1:]

    rules = []
    for line in rule_lines:
        if line:
            x, y = line.split('|')
            x, y = x.strip(), y.strip()
            rules.append((int(x), int(y)))

    updates = []
    for line in update_lines:
        if line:
            pages = [int(x.strip()) for x in line.split(',')]
            updates.append(pages)

    return rules, updates

def is_correctly_ordered(update: List[int], rules: List[Tuple[int,int]]) -> bool:
    """
    Check if an update's pages are in the correct order based on the given rules.

    Parameters:
        update (List[int]): The list of page numbers in the update.
        rules (List[Tuple[int,int]]): The ordering rules as (x, y) pairs.

    Returns:
        bool: True if the update is correctly ordered, False otherwise.
    """
    page_positions = {page: i for i, page in enumerate(update)}
    for (x, y) in rules:
        if x in page_positions and y in page_positions:
            if page_positions[x] > page_positions[y]:
                return False
    return True

def topological_sort(pages: set, relevant_rules: List[Tuple[int,int]]) -> List[int]:
    """
    Perform a topological sort on the given pages based on the relevant_rules.

    Parameters:
        pages (set): The set of pages to order.
        relevant_rules (List[Tuple[int,int]]): Rules that apply to these pages.

    Returns:
        List[int]: A valid ordering of the pages that satisfies all rules.
    """
    graph = defaultdict(list)
    in_degree = {p: 0 for p in pages}

    for x, y in relevant_rules:
        graph[x].append(y)
        in_degree[y] += 1

    queue = deque([node for node in pages if in_degree[node] == 0])
    sorted_order = []
    while queue:
        node = queue.popleft()
        sorted_order.append(node)
        for neighbor in graph[node]:
            in_degree[neighbor] -= 1
            if in_degree[neighbor] == 0:
                queue.append(neighbor)

    return sorted_order

def part_one(rules: List[Tuple[int,int]], updates: List[List[int]]) -> int:
    """
    Sum the middle page of each correctly-ordered update.

    Parameters:
        rules (List[Tuple[int,int]]): The ordering rules.
        updates (List[List[int]]): The list of updates.

    Returns:
        int: The sum of the middle pages of all correctly-ordered updates.
    """
    total = 0
    for upd in updates:
        if is_correctly_ordered(upd, rules):
            middle_index = len(upd) // 2
            total += upd[middle_index]
    return total

def part_two(rules: List[Tuple[int,int]], updates: List[List[int]]) -> int:
    """
    For each incorrectly-ordered update, find the correct order and sum their middle pages.

    Parameters:
        rules (List[Tuple[int,int]]): The ordering rules.
        updates (List[List[int]]): The list of updates.

    Returns:
        int: The sum of the middle pages of all re-ordered incorrect updates.
    """
    incorrect_updates = [upd for upd in updates if not is_correctly_ordered(upd, rules)]

    total = 0
    for upd in incorrect_updates:
        page_set = set(upd)
        relevant_rules = [(x, y) for (x, y) in rules if x in page_set and y in page_set]

        sorted_upd = topological_sort(page_set, relevant_rules)
        middle_index = len(sorted_upd) // 2
        total += sorted_upd[middle_index]

    return total

def main():
    """Execute Day 5 puzzle solution."""
    rules, updates = parse_input("../data/Day5.txt")

    # Part One:
    part_one_result = part_one(rules, updates)
    print("Day 5 - Part 1 Result:", part_one_result)

    # Part Two:
    part_two_result = part_two(rules, updates)
    print("Day 5 - Part 2 Result:", part_two_result)

if __name__ == "__main__":
    main()
