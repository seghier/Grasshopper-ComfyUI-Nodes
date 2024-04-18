# Grasshopper_ComfyUI
Convert and use ComfyUI workflow within Grasshopper

Download and use ComfyUI portable version.
https://github.com/comfyanonymous/ComfyUI

Copy ComfyUI UserObjects folder to:
C:\Users\<yourname>\AppData\Roaming\Grasshopper\UserObjects

Copy GHComfyUI folder to:
C:\Users\<yourname>\AppData\Roaming\Grasshopper\Libraries

The components appears in Params\ComfyUI

Modify the Folder of ComfyUI_windows_portable in run_cpu_gh.bat and run_gpu_gh.bat:

set python_executable=E:\ComfyUI_windows_portable\python_embeded\python.exe
set python_script=E:\ComfyUI_windows_portable\ComfyUI\main.py

And copy the two files to <A>:\ComfyUI_windows_portable\

Copy gh_nodes.py to <A>:\ComfyUI_windows_portable\ComfyUI\custom_nodes
gh_nodes will appears under Grasshopper in nodes menu
There are 7 nodes:
GHPrompt, GHString, GHInteger, GHFloat, GHBool, GHFile, GHLoadImage
when you use them make sure to give them a nickname and id(optional to sort the inputs in Grasshopper)
Nicknames must used: seed, cfg, denoise, steps, width, height, positive, negative, sampler, scheduler

Steps:
1- Create your final workflow in ComfyUI.
2- Save the JSON file without spaces in the name.
3- Save The API JSON file without spaces in the name.
4- Launch ComfyUI: "you can use Run ComfyUI component".
4- In Grasshopper use Json to Python component to convert API JSON file to Python file and save the file.
5- Use Load Model component to choose a model: checkpoint, lora, controlnet ,....etc.
6- Use Samplers and Schedulers components.
6- Use ComfyUI Settings and AddSources components to create the command arguments.
The inputs created automatically when you plug ComfyUI Inputs from Json to Python component to Inputs.
Right click on ComfyUI Settings component to enable Delete inputs, when you unplug the Inputs all other inputs deleted.
Right click on AddSources component to enable Disable update if you want keep the previous sliders, panels ... created.
If GHInteger_seed = -1 this create a random seed, otherwise you can fix it and you can use Sampler seed component.

7- Use Queue Prompt to run the queue, and always check ComfyUI cmd window or http://127.0.0.1:8188 to make sure it running.
8- All images, meshes will saved in the default folder: <A>:\ComfyUI_windows_portable\ComfyUI\output\
Use Image file or Mesh File components to get the latest file created, and use Load Mesh Result component to open the mesh file.

Fill free to fix and improve the componets and the scripts.
I have an old laptop which don't support gpu so i can't give any help for ComfyUI workflows.

