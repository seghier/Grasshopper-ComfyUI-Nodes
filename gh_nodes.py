import torch
from pathlib import Path
from typing import Tuple
import hashlib
from PIL import Image, ImageOps

import numpy as np

import folder_paths

class AnyType(str):
    """A special class that is always equal in not equal comparisons. Credit to pythongosssss"""

    def __ne__(self, __value: object) -> bool:
        return False

any_type = AnyType("*")

class GHNode:
    @classmethod
    def INPUT_TYPES(cls):
        raise NotImplementedError("INPUT_TYPES method must be implemented in subclasses.")

    RETURN_TYPES = (any_type,)
    FUNCTION = "run"
    OUTPUT_NODE = True
    CATEGORY = "Grasshopper"

    def run(self, *args, **kwargs):
        raise NotImplementedError("run method must be implemented in subclasses.")

class GHLoadImage:
    @classmethod
    def INPUT_TYPES(s):     
        return {
            "required": {
                "input_val": ("STRING", {"multiline": False}),
                "invert_mask": ("BOOLEAN", {"default": False}),
                "input_id": ("INT", {"default": 0}),
                "nickname": ("STRING", {"multiline": False}),
            }
        }

    #RETURN_TYPES = ("IMAGE","MASK",)
    RETURN_TYPES = (any_type,any_type,)
    RETURN_NAMES = ("image","mask",)
    FUNCTION = "load_image"
    OUTPUT_NODE = True
    CATEGORY = "Grasshopper"
    
    def load_image(self, input_val, invert_mask, input_id, nickname):
        image_path = GHLoadImage._resolve_path(input_val)

        i = Image.open(image_path).convert("RGBA")  # Open image with alpha channel
        i = ImageOps.exif_transpose(i)
        input_val = i.convert("RGB")  # Convert to RGB for standardization
        input_val = np.array(input_val).astype(np.float32) / 255.0
        input_val = torch.from_numpy(input_val)[None,]

        if 'A' in i.getbands():  # Check if alpha channel exists
            mask = np.array(i.getchannel('A')).astype(np.float32) / 255.0
            mask = 1. - torch.from_numpy(mask)
            if invert_mask:
                mask = 1 - mask
        else:
            mask = torch.zeros(input_val.shape[1:], dtype=torch.float32)  # Create a zero mask if no alpha channel

        return (input_val, mask.unsqueeze(0))

    @staticmethod
    def _resolve_path(input_val):
        image_path = Path(folder_paths.get_annotated_filepath(input_val))
        return image_path

    @classmethod
    def IS_CHANGED(cls, input_val):
        image_path = GHLoadImage._resolve_path(input_val)
        m = hashlib.sha256()
        with open(image_path, 'rb') as f:
            m.update(f.read())
        return m.digest().hex()

    @classmethod
    def VALIDATE_INPUTS(cls, input_val):
        # If image is an output of another node, it will be None during validation
        if input_val is None:
            return True

        image_path = GHLoadImage._resolve_path(input_val)
        if not image_path.exists():
            return "Invalid image path: {}".format(image_path)

        return True
        
class GHFloat(GHNode):
    @classmethod
    def INPUT_TYPES(cls):
        return {
            "required": {
                "input_val": ("FLOAT", {"default": 0.00}),
                "input_id": ("INT", {"default": 0}),
                "nickname": ("STRING", {"multiline": False}),
            }
        }
        
    RETURN_NAMES = ("float",)
    def run(self, input_val, input_id, nickname):
        return (input_val,)

class GHInteger(GHNode):
    @classmethod
    def INPUT_TYPES(cls):
        return {
            "required": {
                "input_val": ("INT", {"default": 0}),
                "input_id": ("INT", {"default": 0}),
                "nickname": ("STRING", {"multiline": False}),
            }
        }
        
    RETURN_NAMES = ("integer",)
    def run(self, input_val, input_id, nickname):
        return (input_val,)

class GHBool(GHNode):
    @classmethod
    def INPUT_TYPES(cls):
        return {
            "required": {
                "input_val": ("BOOLEAN", {"default": False}),
                "input_id": ("INT", {"default": 0}),
                "nickname": ("STRING", {"multiline": False}),
            }
        }
        
    RETURN_NAMES = ("boolean",)
    def run(self, input_val, input_id, nickname):
        return (input_val,)

class GHFile(GHNode):
    @classmethod
    def INPUT_TYPES(cls):
        return {
            "required": {
                "input_val": ("STRING", {"multiline": False}),
                "input_id": ("INT", {"default": 0}),
                "nickname": ("STRING", {"multiline": False}),
            }
        }
        
    RETURN_NAMES = ("file",)
    def run(self, input_val, input_id, nickname):
        return (input_val,)
        
class GHString(GHNode):
    @classmethod
    def INPUT_TYPES(cls):
        return {
            "required": {
                "input_val": ("STRING", {"multiline": False}),
                "input_id": ("INT", {"default": 0}),
                "nickname": ("STRING", {"multiline": False}),
            }
        }
        
    RETURN_NAMES = ("string",)
    def run(self, input_val, input_id, nickname):
        return (input_val,)

class GHPrompt(GHNode):
    @classmethod
    def INPUT_TYPES(cls):
        return {
            "required": {
                "input_val": ("STRING", {"multiline": True}),
                "input_id": ("INT", {"default": 0}),
                "nickname": ("STRING", {"multiline": False}),
            }
        }
        
    RETURN_NAMES = ("prompt",)
    def run(self, input_val, input_id, nickname):
        return (input_val,)
 
NODE_CLASS_MAPPINGS = {
    "GHPrompt": GHPrompt,
    "GHString": GHString,
    "GHInteger": GHInteger,
    "GHFloat": GHFloat,
    "GHBool": GHBool,
    "GHFile": GHFile,
    "GHLoadImage": GHLoadImage,
}

NODE_DISPLAY_NAME_MAPPINGS = {
    "GHPrompt": "GHPrompt",
    "GHString": "GHString",
    "GHInteger": "GHInteger",
    "GHFloat": "GHFloat",
    "GHBool": "GHBool",
    "GHFile": "GHFile",
    "GHLoadImage": "GHLoadImage",
}
