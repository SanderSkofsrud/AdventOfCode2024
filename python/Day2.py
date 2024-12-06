from typing import List

def is_safe_report(report: List[int]) -> bool:
    """
    Determine if a report is "safe". A report is safe if the difference between consecutive
    elements is either always between -3 and -1 (inclusive) or always between 1 and 3 (inclusive).

    Parameters:
        report (List[int]): A list of integers representing the report.

    Returns:
        bool: True if the report is safe, False otherwise.
    """
    differences = [b - a for a, b in zip(report, report[1:])]
    if all(-3 <= d <= -1 for d in differences):
        return True
    if all(1 <= d <= 3 for d in differences):
        return True
    return False

def is_safe_with_dampener(report: List[int]) -> bool:
    """
    Check if a report can be made safe by removing exactly one level (one element from the list).
    If the report is already safe, return True.
    Otherwise, try removing each element and check if it becomes safe.

    Parameters:
        report (List[int]): The report list.

    Returns:
        bool: True if the report can be safe after removing zero or one element, False otherwise.
    """
    if is_safe_report(report):
        return True

    for i in range(len(report)):
        modified_report = report[:i] + report[i+1:]
        if is_safe_report(modified_report):
            return True

    return False

def count_safe_reports(filename: str, with_dampener: bool = False) -> int:
    """
    Count the number of safe reports from a file.

    If with_dampener is False, checks if reports are safe as-is.
    If with_dampener is True, checks if reports can be safe with the dampener applied.

    Parameters:
        filename (str): Path to the file containing reports.
        with_dampener (bool): Whether to check the dampener scenario.

    Returns:
        int: Number of safe reports.
    """
    safe_count = 0
    with open(filename, 'r') as file:
        for line in file:
            report = list(map(int, line.split()))
            if with_dampener:
                if is_safe_with_dampener(report):
                    safe_count += 1
            else:
                if is_safe_report(report):
                    safe_count += 1
    return safe_count

def main():
    """Execute Day 2 puzzle solution."""
    file_path = '../data/Day2.txt'

    # Part 1
    safe_reports_count = count_safe_reports(file_path)
    print(f"Day 2 - Part 1: Number of safe reports: {safe_reports_count}")

    # Part 2
    safe_reports_with_dampener = count_safe_reports(file_path, with_dampener=True)
    print(f"Day 2 - Part 2: Number of safe reports with Dampener: {safe_reports_with_dampener}")

if __name__ == "__main__":
    main()
