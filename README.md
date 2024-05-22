# Grasshopper ComfyUI Nodes
## Convert and Use ComfyUI Workflow within Grasshopper

This repository facilitates the integration of ComfyUI workflow into Grasshopper.

## Installation Steps:

1. Download and use the ComfyUI portable version from https://github.com/comfyanonymous/ComfyUI.
2. Copy `gh_nodes.py` to `<A>:\ComfyUI_windows_portable\ComfyUI\custom_nodes`. The custom nodes will appear under Grasshopper in the nodes menu.

## Available Nodes:
- GHSampler
- GHPrompt
- GHString
- GHInteger
- GHFloat
- GHBool
- GHFile
- LoadImageGH

When using them, make sure to give them a nickname. Required nicknames are: `seed`, `cfg`, `denoise`, `steps`, `width`, `height`, `positive`, `negative`, `sampler`, `scheduler`, .....

## Screenshots:
![Screenshot 2024-04-18 061143](https://github.com/seghier/Grasshopper_ComfyUI/assets/6026588/76a2204e-3891-4204-87d4-6ddaf52703fd)
![Screenshot 2](https://github.com/seghier/Grasshopper_ComfyUI/assets/6026588/65ce6fad-682c-4f4f-8666-fcb9d1bfdc65)

## Usage Steps:
1. Create your final workflow in ComfyUI.
2. Save the JSON file without spaces in the name.
3. Save the API JSON file without spaces in the name.
4. Launch ComfyUI.
5. Use Grasshopper components: [https://www.food4rhino.com/en/app/comfyui-rhino-grasshopper](https://www.food4rhino.com/en/app/comfyui-rhino-grasshopper)
6. Use Load Model component to choose a model: checkpoint, lora, controlnet, etc.
7. Use Samplers and Schedulers components.
9. If `GHInteger_seed = -1`, this creates a random seed; otherwise, you can fix it, and you can use Sampler seed component.
10. Use Queue Prompt to run the queue, and always check ComfyUI cmd window or [http://127.0.0.1:8188](http://127.0.0.1:8188) to make sure it is running.

All images and meshes will be saved in the default folder: `<A>:\ComfyUI_windows_portable\ComfyUI\output\`.

## Important:
- I have an old laptop that doesn't support GPU, so I can't provide any help for ComfyUI workflows.
- If you find this useful: [Buy me a Coffee](https://buymeacoffee.com/seghier?t=true)
