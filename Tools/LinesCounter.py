from os import listdir, getcwd
from os.path import isfile, isdir
from sys import argv


def read_files(directory: str, result: list, extensions: list) -> None:
    for file in listdir(directory):
        actual_file = f"{directory}/{file}"
        if isdir(actual_file):
            read_files(actual_file, result, extensions)
        if isfile(actual_file) and file.split(".")[-1] in extensions:
            result.append(actual_file)


fileExtensions = ["cs"]
if len(argv) > 1:
    fileExtensions.extend(argv[1:])

codeFiles = list()
read_files(getcwd(), codeFiles, fileExtensions)
lines = 0
for f in codeFiles:
    with open(f, 'r') as t:
        lines += len(t.readlines())
print("Opened folder:", getcwd())
print("Files amount:", len(codeFiles))
print("Lines in the project code:", lines)
