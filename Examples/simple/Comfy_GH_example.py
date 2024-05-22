# Created by Seghier Mohamed Abdelaziz - seghierdesign@gmail.com
# C:\Users\archi\source\repos\Grasshopper_ComfyUI\Examples\simple\Comfy_GH_example.json

import json
import sys
from urllib import request
        
def parse_arguments(args):
    GHString_model = ""
    GHInteger_width = ""
    GHInteger_height = ""
    GHInteger_seed = ""
    GHInteger_steps = ""
    GHString_sampler = ""
    GHString_scheduler = ""
    GHFloat_cfg = ""
    GHFloat_denoise = ""
    GHPrompt_positive = ""
    GHPrompt_negative = ""
    GHInteger_batch = ""
    APIjsonfile = ""
    for i in range(len(args)):
        if args[i] == "--GHString_model" and i + 1 < len(args):
            GHString_model = args[i + 1]
        elif args[i] == "--GHInteger_width" and i + 1 < len(args):
            GHInteger_width = args[i + 1]
        elif args[i] == "--GHInteger_height" and i + 1 < len(args):
            GHInteger_height = args[i + 1]
        elif args[i] == "--GHInteger_seed" and i + 1 < len(args):
            GHInteger_seed = args[i + 1]
        elif args[i] == "--GHInteger_steps" and i + 1 < len(args):
            GHInteger_steps = args[i + 1]
        elif args[i] == "--GHString_sampler" and i + 1 < len(args):
            GHString_sampler = args[i + 1]
        elif args[i] == "--GHString_scheduler" and i + 1 < len(args):
            GHString_scheduler = args[i + 1]
        elif args[i] == "--GHFloat_cfg" and i + 1 < len(args):
            GHFloat_cfg = args[i + 1]
        elif args[i] == "--GHFloat_denoise" and i + 1 < len(args):
            GHFloat_denoise = args[i + 1]
        elif args[i] == "--GHPrompt_positive" and i + 1 < len(args):
            GHPrompt_positive = args[i + 1]
        elif args[i] == "--GHPrompt_negative" and i + 1 < len(args):
            GHPrompt_negative = args[i + 1]
        elif args[i] == "--GHInteger_batch" and i + 1 < len(args):
            GHInteger_batch = args[i + 1]
        elif args[i] == "--APIjsonfile" and i + 1 < len(args):
            APIjsonfile = args[i + 1]
    return GHString_model, GHInteger_width, GHInteger_height, GHInteger_seed, GHInteger_steps, GHString_sampler, GHString_scheduler, GHFloat_cfg, GHFloat_denoise, GHPrompt_positive, GHPrompt_negative, GHInteger_batch, APIjsonfile
GHString_model, GHInteger_width, GHInteger_height, GHInteger_seed, GHInteger_steps, GHString_sampler, GHString_scheduler, GHFloat_cfg, GHFloat_denoise, GHPrompt_positive, GHPrompt_negative, GHInteger_batch, APIjsonfile = parse_arguments(sys.argv[1:])

def queue_prompt(prompt):
    p = {"prompt": prompt}
    data = json.dumps(p).encode('utf-8')
    req =  request.Request("http://127.0.0.1:8188/prompt", data=data)
    request.urlopen(req)
    
with open(APIjsonfile, 'r', encoding='utf-8') as file:
    prompt = json.load(file)

# GH inputs
prompt["13"]["inputs"]["model"] = GHString_model
prompt["15"]["inputs"]["width"] = GHInteger_width
prompt["16"]["inputs"]["height"] = GHInteger_height
prompt["17"]["inputs"]["seed"] = GHInteger_seed
prompt["18"]["inputs"]["steps"] = GHInteger_steps
prompt["19"]["inputs"]["sampler"] = GHString_sampler
prompt["20"]["inputs"]["scheduler"] = GHString_scheduler
prompt["21"]["inputs"]["cfg"] = GHFloat_cfg
prompt["22"]["inputs"]["denoise"] = GHFloat_denoise
prompt["23"]["inputs"]["positive"] = GHPrompt_positive
prompt["24"]["inputs"]["negative"] = GHPrompt_negative
prompt["27"]["inputs"]["batch"] = GHInteger_batch

queue_prompt(prompt)
