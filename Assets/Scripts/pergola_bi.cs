using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using UnityEngine;

namespace RESTfulHTTPServer.src.invoker
{
	public class pergola_bi
	{


	}

	class FrameDividers
	{
		//Frame dividers for I.
		//private readonly float MAX_SUPPORTBAR_DIVIDER_DISTANCE = 3000;
		private float _pergola_width;
		private PartSettings _verticalPartSetting { get; }
		private PartSettings _framePartSetting;
		private float _verticalPoleWidth = -1;

		public int numberOfDividerPoles { get; }
		public int numbefOfFields { get; }
		public float each_field_width { get; }
		public FrameDividers(float pergola_width, PartSettings framePartSetting, PartSettings verticalPartSetting, float MAX_SUPPORTBAR_DIVIDER_DISTANCE)
		{
			_pergola_width = pergola_width;
			_verticalPartSetting = verticalPartSetting;
			_framePartSetting = framePartSetting;
			_verticalPoleWidth = verticalPartSetting._part_depth;

			float a = _pergola_width / MAX_SUPPORTBAR_DIVIDER_DISTANCE;
			int divideFrameLengthBy = Convert.ToInt32(Math.Ceiling(a));
			numbefOfFields = divideFrameLengthBy;
			numberOfDividerPoles = numbefOfFields - 1;

			float field_width = pergola_width - (numberOfDividerPoles * _verticalPoleWidth);
			field_width = field_width - Convert.ToSingle(_framePartSetting._part_depth * (2)); //(2) is bcz left and right end frame;
			field_width = field_width / divideFrameLengthBy;
			if (field_width >= 0)
			{
				each_field_width = field_width;
			}
			else
			{
				throw new Exception("Invalid field width");
			}
		}
	}

	public class PartSettings
	{
		string _connectionstring = "";

		public string _part_id;
		public string _part_type;
		public float _part_height;
		public float _part_width;
		public float _part_depth;

		public PartSettings(string part_id, string connectionString)
		{
			_connectionstring = connectionString;
			DataTable dtPartSettings = get_part_measurement(part_id);
			if (dtPartSettings.Rows.Count <= 0) throw new Exception($"Part {part_id} measurement setting data not found.");

			_part_id = dtPartSettings.Rows[0]["part_id"].ToString();
			_part_type = dtPartSettings.Rows[0]["part_type"].ToString();
			_part_height = Convert.ToSingle(dtPartSettings.Rows[0]["part_height"]);
			_part_width = Convert.ToSingle(dtPartSettings.Rows[0]["part_width"]);
			_part_depth = Convert.ToSingle(dtPartSettings.Rows[0]["part_depth"]);
		}

		private DataTable get_part_measurement(string part_id)
		{
			using (SqlConnection con = new SqlConnection(_connectionstring))
			{
				SqlDataAdapter daPartSetting = new SqlDataAdapter();
				con.Open();
				string qry = "";

				qry = $"select part_id, part_type, part_height, part_width, part_depth from tbl_part_setting_header where part_id = '{part_id}'";
				SqlCommand sql = new SqlCommand(qry, con);
				daPartSetting.SelectCommand = sql;
				DataTable dtPartSettings = new DataTable();
				daPartSetting.Fill(dtPartSettings);
				con.Close();
				return dtPartSettings;
			}
		}
	}

	class InnerFieldDividers
	{
		//private readonly float MAX_FIELD_DIVIDER_DISTANCE = 1000;
		decimal L25x25_Accessory_thicknes = 1.2m; //2mm
		decimal L25x25_Accessory_width = 25m;
		decimal u_accessory_thicknes = 0.8m; //m

		private float _fieldWidth;
		private PartSettings _verticalPartSetting { get; }
		private PartSettings _framePartSetting;
		private float _verticalPoleWidth = -1;

		public int numberOfInnerDividerPoles { get; }
		public int numbefOfInnerFields { get; }
		public float each_inner_field_width { get; }
		public InnerFieldDividers(float fieldWidth, PartSettings verticalPartSetting, float MAX_FIELD_DIVIDER_DISTANCE)
		{
			_fieldWidth = fieldWidth;
			_verticalPartSetting = verticalPartSetting;
			_verticalPoleWidth = verticalPartSetting._part_depth;

			float a = _fieldWidth / MAX_FIELD_DIVIDER_DISTANCE;
			int divideFieldLengthBy = Convert.ToInt32(Math.Ceiling(a));
			numbefOfInnerFields = divideFieldLengthBy;
			numberOfInnerDividerPoles = numbefOfInnerFields - 1;

			float field_width = fieldWidth - (numberOfInnerDividerPoles * _verticalPoleWidth);
			field_width = field_width / divideFieldLengthBy;
			if (field_width >= 0)
			{
				each_inner_field_width = field_width;
			}
			else
			{
				throw new Exception("Invalid inner field width");
			}
		}
	}

	public class SupportBarLengths
	{
		public float supportBar_length { get; }
		public float supportWall_length { get; }

		public float supportBar_angle { get; }

		//frameLength is not pergula full length.. Instead the length of pergula where bar should attach.
		public SupportBarLengths(float frameLength)
		{

			//decimal A = frameLength / Convert.ToDecimal(Math.Sin(angle * Math.PI / 180));//converting degree to radians
			float A = frameLength + 500;


			//decimal C = A * Convert.ToDecimal(Math.Cos(angle * Math.PI / 180));//converting degree to radians
			float C = Convert.ToSingle(Math.Sqrt(Math.Pow((Double)A, 2) - Math.Pow((Double)frameLength, 2)));

			float angle = Convert.ToSingle(Math.Acos((Double)frameLength / (Double)A) * 180 / Math.PI);

			supportBar_length = (A - 500);//500.2
			supportWall_length = C;
			supportBar_angle = (float)Math.Ceiling(angle);

		}
	}

	public class FrameDividersForL
	{
		//Frame dividers for L.
		private readonly float MAX_SUPPORTBAR_DIVIDER_DISTANCE = 3000;
		private float _pergola_width;
		private PartSettings _verticalPartSetting { get; }
		private PartSettings _framePartSetting;
		private float _verticalPoleWidth = -1;

		public int numberOfDividerPoles { get; }
		public int numbefOfFields { get; }
		public float each_field_width { get; }
		public FrameDividersForL(float pergola_width, PartSettings framePartSetting, PartSettings verticalPartSetting)
		{
			_pergola_width = pergola_width;
			_verticalPartSetting = verticalPartSetting;
			_framePartSetting = framePartSetting;
			_verticalPoleWidth = verticalPartSetting._part_depth;

			float a = _pergola_width / MAX_SUPPORTBAR_DIVIDER_DISTANCE;
			int divideFrameLengthBy = Convert.ToInt32(Math.Ceiling(a));
			numbefOfFields = divideFrameLengthBy;
			numberOfDividerPoles = numbefOfFields - 1;

			float field_width = pergola_width - (numberOfDividerPoles * _verticalPoleWidth);
			//field_width = field_width - Convert.ToSingle(_framePartSetting._part_depth * (1)); //(1) is bcz right end frame;
			//field_width = field_width - Convert.ToSingle(verticalPartSetting._part_depth * (1)); //(1) is bcz left end is vertical divider;
			field_width = field_width / divideFrameLengthBy;
			if (field_width >= 0)
			{
				each_field_width = field_width;
			}
			else
			{
				throw new Exception("Invalid field width");
			}
		}
	}


}