using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characteristics : MonoBehaviour
{
    //these params vary for each part
    public string part_name_id, part_unique_id, part_type, part_group;
    //these params are constant for a project
    public static string project_unique_id, building_unique_id, element_unique_id, section_name;

    public string left_end_cut_angle = "", right_end_cut_angle = "";
    public string icon_filename;

    public string cube_left_pos="", cube_right_pos="";

    public string previous_arrow_txt_value = null;
    public Transform previous_parent_transform = null;
    public int instantiate = 1;

    public bool step_cut = false;

    public float step_cut_width = 0;
}
