import re

def extract_and_sum_mul_instructions(memory: str) -> int:
    """
    Extract all 'mul(X,Y)' instructions from the given memory string and sum their products.
    This is for Part 1, where all mul instructions are enabled.

    Parameters:
        memory (str): The corrupted memory text.

    Returns:
        int: The sum of products from all valid mul instructions.
    """
    pattern = r"mul\(\d+,\d+\)"
    matches = re.findall(pattern, memory)

    total = 0
    for match in matches:
        numbers = list(map(int, re.findall(r"\d+", match)))
        if len(numbers) == 2:
            total += numbers[0] * numbers[1]
    return total

def extract_and_sum_mul_instructions_with_conditions(memory: str) -> int:
    """
    Extract mul(X,Y) instructions but consider 'do()' and 'don't()' switches.
    When a 'don't()' is encountered, all following mul instructions are ignored
    until a 'do()' is encountered again.

    Parameters:
        memory (str): The corrupted memory text containing instructions.

    Returns:
        int: The sum of products of mul instructions that were enabled.
    """
    mul_pattern = r"mul\(\d+,\d+\)"
    control_pattern = r"do\(\)|don't\(\)"

    mul_matches = re.finditer(mul_pattern, memory)
    control_matches = re.finditer(control_pattern, memory)

    all_matches = sorted(
        list(mul_matches) + list(control_matches), key=lambda match: match.start()
    )

    total = 0
    mul_enabled = True

    for match in all_matches:
        text = match.group()
        if text == "do()":
            mul_enabled = True
        elif text == "don't()":
            mul_enabled = False
        elif mul_enabled and "mul" in text:
            numbers = list(map(int, re.findall(r"\d+", text)))
            if len(numbers) == 2:
                total += numbers[0] * numbers[1]

    return total

def read_memory_from_file(filename: str) -> str:
    """
    Read the entire contents of the file as a single string.

    Parameters:
        filename (str): The path to the input file.

    Returns:
        str: The file contents as a single string.
    """
    with open(filename, 'r') as file:
        return file.read()

def main():
    """Execute Day 3 puzzle solution."""
    file_path = '../data/Day3.txt'
    memory = read_memory_from_file(file_path)

    # Part 1
    part1_result = extract_and_sum_mul_instructions(memory)
    print(f"Day 3 - Part 1: Total sum of valid mul instructions: {part1_result}")

    # Part 2
    part2_result = extract_and_sum_mul_instructions_with_conditions(memory)
    print(f"Day 3 - Part 2: Total sum of enabled mul instructions: {part2_result}")

if __name__ == "__main__":
    main()
