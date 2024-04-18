# Grasshopper_ComfyUI
## Convert and Use ComfyUI Workflow within Grasshopper

This repository facilitates the integration of ComfyUI workflow into Grasshopper.

## Installation Steps:

1. Download and use the ComfyUI portable version from https://github.com/comfyanonymous/ComfyUI.

2. Copy the `ComfyUI UserObjects` folder to: `C:\Users\<yourname>\AppData\Roaming\Grasshopper\UserObjects`.

3. Copy the `GHComfyUI` folder to: `C:\Users\<yourname>\AppData\Roaming\Grasshopper\Libraries`. The components will appear in `Params\ComfyUI`.

4. Modify the folder paths in `run_cpu_gh.bat` and `run_gpu_gh.bat`:
    ```batch
    set python_executable=E:\ComfyUI_windows_portable\python_embeded\python.exe
    set python_script=E:\ComfyUI_windows_portable\ComfyUI\main.py
    ```
   Copy the modified files to `<A>:\ComfyUI_windows_portable\`.

5. Copy `gh_nodes.py` to `<A>:\ComfyUI_windows_portable\ComfyUI\custom_nodes`. The custom nodes will appear under Grasshopper in the nodes menu.

## Available Nodes:
- GHPrompt
- GHString
- GHInteger
- GHFloat
- GHBool
- GHFile
- GHLoadImage

When using them, make sure to give them a nickname and an id (optional to sort the inputs in Grasshopper). Required nicknames are: `seed`, `cfg`, `denoise`, `steps`, `width`, `height`, `positive`, `negative`, `sampler`, `scheduler`.

## Screenshots:
![Screenshot 1](https://github.com/seghier/Grasshopper_ComfyUI/assets/6026588/f9a3c4e7-5a18-4522-a17d-6585b2df2366)
![Screenshot 2](https://github.com/seghier/Grasshopper_ComfyUI/assets/6026588/65ce6fad-682c-4f4f-8666-fcb9d1bfdc65)

## Usage Steps:
1. Create your final workflow in ComfyUI.
2. Save the JSON file without spaces in the name.
3. Save the API JSON file without spaces in the name.
4. Launch ComfyUI: "you can use Run ComfyUI component".
5. In Grasshopper, use Json to Python component to convert API JSON file to Python file and save the file.
6. Use Load Model component to choose a model: checkpoint, lora, controlnet, etc.
7. Use Samplers and Schedulers components.
8. Use ComfyUI Settings and AddSources components to create the command arguments.
   - The inputs are created automatically when you plug ComfyUI Inputs from Json to Python component to Inputs.
   - Right-click on ComfyUI Settings component to enable Delete inputs; when you unplug the Inputs, all other inputs are deleted.
   - Right-click on AddSources component to enable Disable update if you want to keep the previously created sliders, panels, etc.
9. If `GHInteger_seed = -1`, this creates a random seed; otherwise, you can fix it, and you can use Sampler seed component.
10. Use Queue Prompt to run the queue, and always check ComfyUI cmd window or [http://127.0.0.1:8188](http://127.0.0.1:8188) to make sure it is running.

All images and meshes will be saved in the default folder: `<A>:\ComfyUI_windows_portable\ComfyUI\output\`. Use Image file or Mesh File components to get the latest file created, and use Load Mesh Result component to open the mesh file.

## Important:
- I have an old laptop that doesn't support GPU, so I can't provide any help for ComfyUI workflows.
- Feel free to fix and improve the components and the scripts.
