import sys
import json


def process_words(file_path):
    word_lengths = {}

    with open(file_path, "r", encoding="utf-8") as file:
        words = file.readlines()

        for word in words:
            word_lengths.update({word: len(word)})
            # word_lengths[word] = len(word)

    return word_lengths


def convert_to_json(word_lengths, output_file):
    with open(output_file, "w", encoding="utf-8") as file:
        json.dump(word_lengths, file, ensure_ascii=False)


if __name__ == "__main__":
    # Check if the correct number of arguments is provided
    if len(sys.argv) != 3:
        print("Usage: python Process.py <input_file> <output_file>")
    else:
        # File path of the input text file
        input_file_path = sys.argv[1]

        # File path of the output JSON file
        output_file_path = sys.argv[2]

        # Process the words and get the word lengths
        word_lengths = process_words(input_file_path)

        # Convert the word lengths to JSON and write to a file
        convert_to_json(word_lengths, output_file_path)
