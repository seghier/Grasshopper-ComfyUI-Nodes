# Created by Seghier Mohamed Abdelaziz - seghierdesign@gmail.com
# C:\Users\archi\source\repos\Grasshopper_ComfyUI\Examples\Text23D\text23d_api.json

import json
import sys
from urllib import request
        
def parse_arguments(args):
    GHString_model = ""
    GHInteger_imageWH = ""
    GHInteger_resolution = ""
    GHInteger_seed = ""
    GHFloat_cfg = ""
    GHString_sampler = ""
    GHInteger_steps = ""
    GHFloat_denoise = ""
    GHString_scheduler = ""
    GHPrompt_positive = ""
    GHPrompt_negative = ""
    GHString_sdversion = ""
    APIjsonfile = ""
    for i in range(len(args)):
        if args[i] == "--GHString_model" and i + 1 < len(args):
            GHString_model = args[i + 1]
        elif args[i] == "--GHInteger_imageWH" and i + 1 < len(args):
            GHInteger_imageWH = args[i + 1]
        elif args[i] == "--GHInteger_resolution" and i + 1 < len(args):
            GHInteger_resolution = args[i + 1]
        elif args[i] == "--GHInteger_seed" and i + 1 < len(args):
            GHInteger_seed = args[i + 1]
        elif args[i] == "--GHFloat_cfg" and i + 1 < len(args):
            GHFloat_cfg = args[i + 1]
        elif args[i] == "--GHString_sampler" and i + 1 < len(args):
            GHString_sampler = args[i + 1]
        elif args[i] == "--GHInteger_steps" and i + 1 < len(args):
            GHInteger_steps = args[i + 1]
        elif args[i] == "--GHFloat_denoise" and i + 1 < len(args):
            GHFloat_denoise = args[i + 1]
        elif args[i] == "--GHString_scheduler" and i + 1 < len(args):
            GHString_scheduler = args[i + 1]
        elif args[i] == "--GHPrompt_positive" and i + 1 < len(args):
            GHPrompt_positive = args[i + 1]
        elif args[i] == "--GHPrompt_negative" and i + 1 < len(args):
            GHPrompt_negative = args[i + 1]
        elif args[i] == "--GHString_sdversion" and i + 1 < len(args):
            GHString_sdversion = args[i + 1]
        elif args[i] == "--APIjsonfile" and i + 1 < len(args):
            APIjsonfile = args[i + 1]
    return GHString_model, GHInteger_imageWH, GHInteger_resolution, GHInteger_seed, GHFloat_cfg, GHString_sampler, GHInteger_steps, GHFloat_denoise, GHString_scheduler, GHPrompt_positive, GHPrompt_negative, GHString_sdversion, APIjsonfile
GHString_model, GHInteger_imageWH, GHInteger_resolution, GHInteger_seed, GHFloat_cfg, GHString_sampler, GHInteger_steps, GHFloat_denoise, GHString_scheduler, GHPrompt_positive, GHPrompt_negative, GHString_sdversion, APIjsonfile = parse_arguments(sys.argv[1:])

def queue_prompt(prompt):
    p = {"prompt": prompt}
    data = json.dumps(p).encode('utf-8')
    req =  request.Request("http://127.0.0.1:8188/prompt", data=data)
    request.urlopen(req)
    
with open(APIjsonfile, 'r', encoding='utf-8') as file:
    prompt = json.load(file)

# GH inputs
prompt["30"]["inputs"]["model"] = GHString_model
prompt["31"]["inputs"]["imageWH"] = GHInteger_imageWH
prompt["32"]["inputs"]["resolution"] = GHInteger_resolution
prompt["36"]["inputs"]["seed"] = GHInteger_seed
prompt["37"]["inputs"]["cfg"] = GHFloat_cfg
prompt["38"]["inputs"]["sampler"] = GHString_sampler
prompt["39"]["inputs"]["steps"] = GHInteger_steps
prompt["40"]["inputs"]["denoise"] = GHFloat_denoise
prompt["41"]["inputs"]["scheduler"] = GHString_scheduler
prompt["58"]["inputs"]["positive"] = GHPrompt_positive
prompt["59"]["inputs"]["negative"] = GHPrompt_negative
prompt["60"]["inputs"]["sdversion"] = GHString_sdversion

queue_prompt(prompt)
