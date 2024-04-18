# Created by Seghier Mohamed Abdelaziz - seghierdesign@gmail.com
        
# C:\Users\archi\source\repos\Grasshopper_ComfyUI\Examples\ComfyUi3D\create_3d.json
import json
import sys
from urllib import request
        
def parse_arguments(args):
    GHFile_model = ""
    GHInteger_resolution = ""
    GHBool_invertmask = ""
    GHFile_imagefile = ""
    APIjsonfile = ""
    for i in range(len(args)):
        if args[i] == "--GHFile_model" and i + 1 < len(args):
            GHFile_model = args[i + 1]
        elif args[i] == "--GHInteger_resolution" and i + 1 < len(args):
            GHInteger_resolution = args[i + 1]
        elif args[i] == "--GHBool_invertmask" and i + 1 < len(args):
            GHBool_invertmask = args[i + 1]
        elif args[i] == "--GHFile_imagefile" and i + 1 < len(args):
            GHFile_imagefile = args[i + 1]
        elif args[i] == "--APIjsonfile" and i + 1 < len(args):
            APIjsonfile = args[i + 1]
    return GHFile_model, GHInteger_resolution, GHBool_invertmask, GHFile_imagefile, APIjsonfile
GHFile_model, GHInteger_resolution, GHBool_invertmask, GHFile_imagefile, APIjsonfile = parse_arguments(sys.argv[1:])

def queue_prompt(prompt):
    p = {"prompt": prompt}
    data = json.dumps(p).encode('utf-8')
    req =  request.Request("http://127.0.0.1:8188/prompt", data=data)
    request.urlopen(req)
    
with open(APIjsonfile, 'r', encoding='utf-8') as file:
    prompt = json.load(file)

# GH inputs
#class type: GHBool
prompt["59"]["inputs"]["input_val"] = GHBool_invertmask
#class type: GHInteger
prompt["25"]["inputs"]["input_val"] = GHInteger_resolution
#class type: GHFile
prompt["62"]["inputs"]["input_val"] = GHFile_imagefile
prompt["9"]["inputs"]["input_val"] = GHFile_model

queue_prompt(prompt)
