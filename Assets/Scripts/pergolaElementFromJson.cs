using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PergolaMeasurement
{
    public string project_unique_id { get; set; }
    public string building_unique_id { get; set; }
    public string element_unique_id { get; set; }
    public string side_name { get; set; }
    public double side_value { get; set; }
    public int is_fixed_to_wall { get; set; }
}

public class PergolaDetails
{
    public string project_unique_id { get; set; }
    public string building_unique_id { get; set; }
    public string element_unique_id { get; set; }
    public string element_shape { get; set; }
    public int element_shape_id { get; set; }
    public string height_from_groundOrroof { get; set; }
    public double height_from_value { get; set; }
    public string frame_type { get; set; }
    public string frame_color_texture { get; set; }
    public string divider_type { get; set; }
    public string support_line_placement { get; set; }
    public string rafafa_type { get; set; }
    public string rafafa_color_texture { get; set; }
    public int rafafa_spacing { get; set; }
    public string rafafa_placement_type { get; set; }
    public int rafafa_direction_id { get; set; }
    public int rafafa_alignment_id { get; set; }
    public int rafafa_light_direction_id { get; set; }
    public int wall_type_id { get; set; }
    public List<PergolaMeasurement> pergola_measurements { get; set; }
}

public class Point
{
    public string unity_heirarchy_name { get; set; }
    public int unity_heirarchy_level { get; set; }
    public float pos_x { get; set; }
    public float pos_y { get; set; }
    public float pos_z { get; set; }
    public float rot_x { get; set; }
    public float rot_y { get; set; }
    public float rot_z { get; set; }
    public float scale_x { get; set; }
    public float scale_y { get; set; }
    public float scale_z { get; set; }
    public string part_name_id { get; set; }
    public string project_unique_id { get; set; }
    public string building_unique_id { get; set; }
    public string element_unique_id { get; set; }
    public string section_name { get; set; }
    public string part_unique_id { get; set; }
    public string part_type { get; set; }
    public string part_group { get; set; }
    public string left_end_cut_angle { get; set; }
    public string right_end_cut_angle { get; set; }
    public string icon_filename { get; set; }
    public int instantiate { get; set; }
}

public class pergolaElementFromJson
{
    public string project_unique_id { get; set; }
    public string building_unique_id { get; set; }
    public string element_unique_id { get; set; }
    public string element_type { get; set; }
    public int element_stage_id { get; set; }
    public string element_stage_name { get; set; }
    public string element_stage_deadline { get; set; }
    public double element_total_area { get; set; }
    public string element_name { get; set; }
    public string remark { get; set; }
    public int is_existing_element { get; set; }
    public string existing_element_pdf_path { get; set; }
    public DateTime element_created_datetime { get; set; }
    public string order_number { get; set; }
    public PergolaDetails pergolaDetails { get; set; }
    public object louverDetails { get; set; }
    public List<Point> points { get; set; }
}