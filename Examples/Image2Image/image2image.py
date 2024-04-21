# Created by Seghier Mohamed Abdelaziz - seghierdesign@gmail.com
        
# C:\Users\archi\source\repos\Grasshopper_ComfyUI\Examples\Image2Image\image2image.json
import json
import sys
from urllib import request
        
def parse_arguments(args):
    GHString_model = ""
    GHString_contolnetdepth = ""
    GHString_stylemodel = ""
    GHString_contolnetsketch = ""
    GHString_clipvision = ""
    GHString_scheduler = ""
    GHString_sampler = ""
    GHPrompt_positive = ""
    GHFile_imagedepth = ""
    GHFile_imagestyle = ""
    GHFile_imagesketch = ""
    GHInteger_batch = ""
    GHInteger_steps = ""
    GHInteger_seed = ""
    GHFloat_controlnetsketchstrength = ""
    GHFloat_controlnetdepthstrength = ""
    GHFloat_denoise = ""
    GHFloat_cfg = ""
    APIjsonfile = ""
    for i in range(len(args)):
        if args[i] == "--GHString_model" and i + 1 < len(args):
            GHString_model = args[i + 1]
        elif args[i] == "--GHString_contolnetdepth" and i + 1 < len(args):
            GHString_contolnetdepth = args[i + 1]
        elif args[i] == "--GHString_stylemodel" and i + 1 < len(args):
            GHString_stylemodel = args[i + 1]
        elif args[i] == "--GHString_contolnetsketch" and i + 1 < len(args):
            GHString_contolnetsketch = args[i + 1]
        elif args[i] == "--GHString_clipvision" and i + 1 < len(args):
            GHString_clipvision = args[i + 1]
        elif args[i] == "--GHString_scheduler" and i + 1 < len(args):
            GHString_scheduler = args[i + 1]
        elif args[i] == "--GHString_sampler" and i + 1 < len(args):
            GHString_sampler = args[i + 1]
        elif args[i] == "--GHPrompt_positive" and i + 1 < len(args):
            GHPrompt_positive = args[i + 1]
        elif args[i] == "--GHFile_imagedepth" and i + 1 < len(args):
            GHFile_imagedepth = args[i + 1]
        elif args[i] == "--GHFile_imagestyle" and i + 1 < len(args):
            GHFile_imagestyle = args[i + 1]
        elif args[i] == "--GHFile_imagesketch" and i + 1 < len(args):
            GHFile_imagesketch = args[i + 1]
        elif args[i] == "--GHInteger_batch" and i + 1 < len(args):
            GHInteger_batch = args[i + 1]
        elif args[i] == "--GHInteger_steps" and i + 1 < len(args):
            GHInteger_steps = args[i + 1]
        elif args[i] == "--GHInteger_seed" and i + 1 < len(args):
            GHInteger_seed = args[i + 1]
        elif args[i] == "--GHFloat_controlnetsketchstrength" and i + 1 < len(args):
            GHFloat_controlnetsketchstrength = args[i + 1]
        elif args[i] == "--GHFloat_controlnetdepthstrength" and i + 1 < len(args):
            GHFloat_controlnetdepthstrength = args[i + 1]
        elif args[i] == "--GHFloat_denoise" and i + 1 < len(args):
            GHFloat_denoise = args[i + 1]
        elif args[i] == "--GHFloat_cfg" and i + 1 < len(args):
            GHFloat_cfg = args[i + 1]
        elif args[i] == "--APIjsonfile" and i + 1 < len(args):
            APIjsonfile = args[i + 1]
    return GHString_model, GHString_contolnetdepth, GHString_stylemodel, GHString_contolnetsketch, GHString_clipvision, GHString_scheduler, GHString_sampler, GHPrompt_positive, GHFile_imagedepth, GHFile_imagestyle, GHFile_imagesketch, GHInteger_batch, GHInteger_steps, GHInteger_seed, GHFloat_controlnetsketchstrength, GHFloat_controlnetdepthstrength, GHFloat_denoise, GHFloat_cfg, APIjsonfile
GHString_model, GHString_contolnetdepth, GHString_stylemodel, GHString_contolnetsketch, GHString_clipvision, GHString_scheduler, GHString_sampler, GHPrompt_positive, GHFile_imagedepth, GHFile_imagestyle, GHFile_imagesketch, GHInteger_batch, GHInteger_steps, GHInteger_seed, GHFloat_controlnetsketchstrength, GHFloat_controlnetdepthstrength, GHFloat_denoise, GHFloat_cfg, APIjsonfile = parse_arguments(sys.argv[1:])

def queue_prompt(prompt):
    p = {"prompt": prompt}
    data = json.dumps(p).encode('utf-8')
    req =  request.Request("http://127.0.0.1:8188/prompt", data=data)
    request.urlopen(req)
    
with open(APIjsonfile, 'r', encoding='utf-8') as file:
    prompt = json.load(file)

# GH inputs
#class type: GHString
prompt["68"]["inputs"]["input_val"] = GHString_model
prompt["70"]["inputs"]["input_val"] = GHString_contolnetdepth
prompt["72"]["inputs"]["input_val"] = GHString_stylemodel
prompt["69"]["inputs"]["input_val"] = GHString_contolnetsketch
prompt["71"]["inputs"]["input_val"] = GHString_clipvision
prompt["84"]["inputs"]["input_val"] = GHString_scheduler
prompt["83"]["inputs"]["input_val"] = GHString_sampler
#class type: GHPrompt
prompt["58"]["inputs"]["input_val"] = GHPrompt_positive
#class type: GHFile
prompt["75"]["inputs"]["input_val"] = GHFile_imagedepth
prompt["74"]["inputs"]["input_val"] = GHFile_imagestyle
prompt["73"]["inputs"]["input_val"] = GHFile_imagesketch
#class type: GHInteger
prompt["76"]["inputs"]["input_val"] = GHInteger_batch
prompt["80"]["inputs"]["input_val"] = GHInteger_steps
prompt["79"]["inputs"]["input_val"] = GHInteger_seed
#class type: GHFloat
prompt["78"]["inputs"]["input_val"] = GHFloat_controlnetsketchstrength
prompt["77"]["inputs"]["input_val"] = GHFloat_controlnetdepthstrength
prompt["82"]["inputs"]["input_val"] = GHFloat_denoise
prompt["81"]["inputs"]["input_val"] = GHFloat_cfg

queue_prompt(prompt)
