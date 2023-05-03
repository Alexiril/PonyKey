from os import listdir, chdir, remove
from os.path import exists, join, isdir, isfile
from sys import argv
from tarfile import open as taropen

from yaml import load, FullLoader


def ClearBuildTime(buildTimeFolder: str):
    for path in listdir(buildTimeFolder):
        remove(join(buildTimeFolder, path))

    with open(join(buildTimeFolder, "BuildingScenes.cs"), "w") as f:
        f.write("""/*     !Temporary file!

        ---------------
        This file was generated using automating tools.
        Please, don't change it yourself, your actions won't be saved.
    */

    using System.Collections.Generic;
    using Engine.BaseTypes;

    namespace Game.BuildTime;

    public static class BuildingScenes
    {
        public static List<ILevel> Scenes => new();
    }
    """)


def CountLines(rootFolder: str, extensions: list = []):
    def read_files(directory: str, result: list, extensions: list) -> None:
        for file in listdir(directory):
            actual_file = f"{directory}/{file}"
            if isdir(actual_file):
                read_files(actual_file, result, extensions)
            if isfile(actual_file) and file.split(".")[-1] in extensions:
                result.append(actual_file)

    fileExtensions = ["cs"]
    fileExtensions.extend(extensions)

    codeFiles = list()
    read_files(rootFolder, codeFiles, fileExtensions)
    lines = 0
    chars = 0
    for f in codeFiles:
        with open(f, 'r') as t:
            text = t.readlines()
            lines += len(text)
            chars += len("".join(text))
    print("Opened folder:", rootFolder)
    print("Files amount:", len(codeFiles))
    print("Lines in the project code:", lines)
    print("Characters in the project code:", chars)


def PackAssets(startFolder: str, contentFolder: str):
    contentFolder = join(startFolder, contentFolder)
    with taropen(join(startFolder, "assets.dat"), "w:gz") as archive:
        chdir(contentFolder)
        for file in listdir(contentFolder):
            archive.add(file)
    print(f"All files and folders from {contentFolder} were added to the archive.")


def AssembleScenes(scenesMapAddress: str, gameFolder: str, buildTimeFolder: str):
    def generate_buildingscenes(scenes: list[str]) -> None:
        with open(join(buildTimeFolder, "BuildingScenes.cs"), "w") as f:
            header = """/*     !Temporary file!

        ---------------
        This file was generated using automating tools.
        Please, don't change it yourself, your actions won't be saved.
    */

    using System.Collections.Generic;
    using Engine.BaseTypes;

    namespace Game.BuildTime;

    public static class BuildingScenes
    {
        public static List<ILevel> Scenes => new()
        {
    """
            footer = """    };
    }
    """
            f.write(header)
            for i in scenes:
                if (exists(join(gameFolder, i + ".yaml"))):
                    f.write(" " * 8 + f"new {i.split('/')[-1]}Scene(),\n")
            f.write(footer)

    def load_asset(asset: dict) -> str:
        if asset["type"] == "texture-svg":
            return f"""SvgConverter.LoadSvg({asset["assetName"]}, {asset["size"]})"""
        elif asset["type"] == "animation-svg":
            return f"""SvgConverter.LoadSvgAnimation({asset["assetName"]}, {asset["size"]})"""
        else:
            params = ""
            for i in asset:
                if i != "type":
                    params += f"{i}:{asset[i]},"
            return f"""ArchivedContent.LoadContent<{asset["type"]}>({params.removesuffix(",")})"""

    def generate_object(key: str, obj: dict, assets: dict) -> str:
        result = f"private static GameObject {key} => new GameObject(\"{key}\")\n"
        for component in obj:
            if component != "GameObject":
                result += f".AddComponent<{component}>()\n"
            else:
                result += f".gameObject"
            if obj[component] != None:
                for settings in obj[component]:
                    settingValue = obj[component][settings]
                    if type(settingValue) == str:
                        settingValue = settingValue if settingValue[
                            0] != "%" else assets[settingValue[1:]]
                    if type(settingValue) == bool:
                        settingValue = str(settingValue).lower()
                    result += f".Set{settings}({settingValue})\n"
        result += ".gameObject;\n"
        return result

    def generate_scene(scene: str) -> None:
        with open(f"{join(buildTimeFolder, scene.split('/')[-1])}.cs", 'w') as result:
            with open(f"{join(gameFolder, scene)}.yaml", 'r') as readFrom:
                sceneData = load(readFrom.read().replace(
                    "@", "EGame."), FullLoader)
            assets = {}
            header = f"""using Engine.BaseComponents;
    using Engine.BaseSystems;
    using Engine.BaseTypes;
    using Game.Components.Common;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using EGame = Engine.BaseSystems.Game;

    namespace Game.BuildTime;

    internal class {scene.split('/')[-1]}Scene : ILevel
    """ + "{\n"
            for asset in sceneData["Assets"]:
                assets[asset] = load_asset(sceneData["Assets"][asset])
            getSceneBlock = f"""public Scene GetScene()=>new Scene("{scene.split('/')[-1]}").SetBackgroundColor({sceneData["BackgroundColor"]})\n"""
            for object in sceneData["Hierarchy"]:
                getSceneBlock += f".AddGameObject({object})\n"
            getSceneBlock += ".ActualScene;\n"
            result.write(header)
            result.write(getSceneBlock)
            for obj in sceneData["Game objects"]:
                result.write(generate_object(
                    obj, sceneData["Game objects"][obj], assets))
            result.write("}")

    print("Scene assembler script v1.0")

    with open(scenesMapAddress, "r") as f:
        scenes = load(f.read(), FullLoader)

    generate_buildingscenes(scenes)

    for i in scenes:
        if (exists(join(gameFolder, i + ".yaml"))):
            generate_scene(i)
        else:
            print(f"{i} wasn't found :()")

    print("Scenes are assembled")

args = argv[1].replace('"', "").split(";")

if len(args) < 4:
    print(f"""Sorry. This is not how to use that script.
You need to start it with the building params.
Using example: ponykeybuilder.py [prebuild | postbuild] rootFolder gameFolder outFolder
Arguments ({len(args)}): {args}""")
    exit(-1)

phase = args[0]
rootFolder = args[1]
gameFolder = join(rootFolder, args[2])
outFolder = join(gameFolder, args[3])
buildTimeFolder = join(gameFolder, "BuildTime")
print("Starting with:")
print(f"Phase = {phase}")
print(f"Root folder = {rootFolder}")
print(f"Game folder = {gameFolder}")
print(f"Out folder = {outFolder}")
print(f"BuildTime folder = {buildTimeFolder}")

if (phase == "prebuild"):
    #AssembleScenes("Scenes.yaml", gameFolder, buildTimeFolder)
    pass
elif (phase == "postbuild"):
    PackAssets(outFolder, join(outFolder, "Content"))
    CountLines(rootFolder, ["yaml", "json", "py"])
    #ClearBuildTime(buildTimeFolder)
else:
    raise AssertionError("Not correct build phase.")
