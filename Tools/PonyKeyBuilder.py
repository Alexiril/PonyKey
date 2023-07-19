from os import listdir, chdir, mkdir
from os.path import join, isdir, isfile
from sys import argv
from tarfile import open as taropen

def count_lines(root_folder: str, extensions=None):
    if extensions is None:
        extensions = []

    def read_files(directory: str, result: list, file_extensions: list) -> None:
        last = directory.split("/")[-1]
        if last in ["build", "bin", "obj"] or last[0] == ".":
            return
        for file in listdir(directory):
            actual_file = f"{directory}/{file}"
            if isdir(actual_file):
                read_files(actual_file, result, file_extensions)
            if isfile(actual_file) and file.split(".")[-1] in file_extensions:
                result.append(actual_file)

    code_files = list()
    read_files(root_folder, code_files, extensions)
    lines = 0
    chars = 0
    for f in code_files:
        with open(f, 'r') as t:
            text = t.readlines()
            lines += len(text)
            chars_inside = len("".join(text))
            chars += chars_inside
            print(f"File {f} contains {len(text)} lines and {chars_inside} characters.")
    print("Opened folder:", root_folder)
    print("Files amount:", len(code_files))
    print("Lines in the project code:", lines)
    print("Characters in the project code:", chars)


def pack_assets(start_folder: str, content_folder: str):
    content_folder = join(start_folder, content_folder)
    with taropen(join(start_folder, "assets.dat"), "w:gz") as archive:
        chdir(content_folder)
        for file in listdir(content_folder):
            archive.add(file)
    print(f"All files and folders from {content_folder} were added to the archive.")


args = argv[1].replace('"', "").split(";")

if len(args) < 4:
    print(f"""Sorry. This is not how to use that script.
You need to start it with the building params.
Using example: ponykeybuilder.py "pre|post";"rootFolder";"gameFolder";"outFolder"
Arguments ({len(args)}): {args}""")
    exit(-3)

phase = args[0]
rootFolder = args[1]
projectFolder = join(rootFolder, args[2])
outFolder = join(projectFolder, args[3])
buildTimeFolder = join(projectFolder, "BuildTime")
print("Starting with:")
print(f"Phase = {phase}")
print(f"Root folder = {rootFolder}")
print(f"Project folder = {projectFolder}")
print(f"Out folder = {outFolder}")
print(f"BuildTime folder = {buildTimeFolder}")

if phase == "post":
    pack_assets(outFolder, join(outFolder, "Content"))
    count_lines(rootFolder, ["cs", "py", "json"])
elif phase == "pre":
    if isdir(join(projectFolder, ".config")) and \
        isfile(join(projectFolder, ".config", "dotnet-tools.json")):
        print("Dotnet tools json exists. Probably correct.")
    elif not isdir(join(projectFolder, ".config")):
        mkdir(join(projectFolder, ".config"))
    if not isfile(join(projectFolder, ".config", "dotnet-tools.json")):
        with open(join(projectFolder, ".config", "dotnet-tools.json"), 'w') as json:
            json.write("""{"version":1,"isRoot":true,"tools":{"dotnet-mgcb":{"version": "3.8.1.303","commands":["mgcb"]},"dotnet-mgcb-editor":{"version":"3.8.1.303","commands":["mgcb-editor"]},"dotnet-mgcb-editor-linux":{"version":"3.8.1.303","commands":["mgcb-editor-linux"]},"dotnet-mgcb-editor-windows":{"version":"3.8.1.303","commands":["mgcb-editor-windows"]},"dotnet-mgcb-editor-mac":{"version":"3.8.1.303","commands":["mgcb-editor-mac"]}}}""")
            print("Dotnet tools json manifest was created.")