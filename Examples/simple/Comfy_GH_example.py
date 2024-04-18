# Created by Seghier Mohamed Abdelaziz - seghierdesign@gmail.com
        
# H:\Comfy_GH\simple\Comfy_GH_example.json
import json
import sys
from urllib import request
        
def parse_arguments(args):
    GHPrompt_positive = ""
    GHPrompt_negative = ""
    GHString_model = ""
    GHInteger_width = ""
    GHInteger_height = ""
    GHInteger_batch = ""
    GHInteger_seed = ""
    GHInteger_steps = ""
    GHFloat_cfg = ""
    GHFloat_denoise = ""
    GHString_sampler = ""
    GHString_scheduler = ""
    APIjsonfile = ""
    for i in range(len(args)):
        if args[i] == "--GHPrompt_positive" and i + 1 < len(args):
            GHPrompt_positive = args[i + 1]
        elif args[i] == "--GHPrompt_negative" and i + 1 < len(args):
            GHPrompt_negative = args[i + 1]
        elif args[i] == "--GHString_model" and i + 1 < len(args):
            GHString_model = args[i + 1]
        elif args[i] == "--GHInteger_width" and i + 1 < len(args):
            GHInteger_width = args[i + 1]
        elif args[i] == "--GHInteger_height" and i + 1 < len(args):
            GHInteger_height = args[i + 1]
        elif args[i] == "--GHInteger_batch" and i + 1 < len(args):
            GHInteger_batch = args[i + 1]
        elif args[i] == "--GHInteger_seed" and i + 1 < len(args):
            GHInteger_seed = args[i + 1]
        elif args[i] == "--GHInteger_steps" and i + 1 < len(args):
            GHInteger_steps = args[i + 1]
        elif args[i] == "--GHFloat_cfg" and i + 1 < len(args):
            GHFloat_cfg = args[i + 1]
        elif args[i] == "--GHFloat_denoise" and i + 1 < len(args):
            GHFloat_denoise = args[i + 1]
        elif args[i] == "--GHString_sampler" and i + 1 < len(args):
            GHString_sampler = args[i + 1]
        elif args[i] == "--GHString_scheduler" and i + 1 < len(args):
            GHString_scheduler = args[i + 1]
        elif args[i] == "--APIjsonfile" and i + 1 < len(args):
            APIjsonfile = args[i + 1]
    return GHPrompt_positive, GHPrompt_negative, GHString_model, GHInteger_width, GHInteger_height, GHInteger_batch, GHInteger_seed, GHInteger_steps, GHFloat_cfg, GHFloat_denoise, GHString_sampler, GHString_scheduler, APIjsonfile
GHPrompt_positive, GHPrompt_negative, GHString_model, GHInteger_width, GHInteger_height, GHInteger_batch, GHInteger_seed, GHInteger_steps, GHFloat_cfg, GHFloat_denoise, GHString_sampler, GHString_scheduler, APIjsonfile = parse_arguments(sys.argv[1:])

def queue_prompt(prompt):
    p = {"prompt": prompt}
    data = json.dumps(p).encode('utf-8')
    req =  request.Request("http://127.0.0.1:8188/prompt", data=data)
    request.urlopen(req)
    
with open(APIjsonfile, 'r', encoding='utf-8') as file:
    prompt = json.load(file)

# GH inputs
#class type: GHString
prompt["13"]["inputs"]["input_val"] = GHString_model
prompt["19"]["inputs"]["input_val"] = GHString_sampler
prompt["20"]["inputs"]["input_val"] = GHString_scheduler
#class type: GHInteger
prompt["17"]["inputs"]["input_val"] = GHInteger_seed
prompt["27"]["inputs"]["input_val"] = GHInteger_batch
prompt["15"]["inputs"]["input_val"] = GHInteger_width
prompt["18"]["inputs"]["input_val"] = GHInteger_steps
prompt["16"]["inputs"]["input_val"] = GHInteger_height
#class type: GHPrompt
prompt["24"]["inputs"]["input_val"] = GHPrompt_negative
prompt["23"]["inputs"]["input_val"] = GHPrompt_positive
#class type: GHFloat
prompt["22"]["inputs"]["input_val"] = GHFloat_denoise
prompt["21"]["inputs"]["input_val"] = GHFloat_cfg

queue_prompt(prompt)
