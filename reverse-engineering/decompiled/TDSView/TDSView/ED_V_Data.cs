using System;

namespace TDSView
{
	public class ED_V_Data
	{
		public ushort event_id;

		public byte limit;

		public uint reference_nr;

		public byte vehicle_pos;

		public byte process_id;

		public byte subsystem_nr;

		public byte location;

		public byte prio;

		public ushort errorcode_0;

		public ushort errorcode_1;

		public ushort errorcode_2;

		public ushort errorcode_3;

		public byte acknow_0;

		public byte acknow_1;

		public byte acknow_2;

		public byte acknow_3;

		public byte err_code_mism;

		public byte active;

		public byte deleted;

		public byte uploaded;

		public ushort event_cnt;

		public uint start_time_sec;

		public ushort start_time_rel;

		public DateTime start_time;

		public uint end_time_sec;

		public ushort end_time_rel;

		public DateTime end_time;

		public float latitude;

		public float longitude;

		public float altitude;

		public float speed;

		public float heading;

		public uint UTC_time;

		public uint odometer;

		public uint trip;

		public uint reserved_0;

		public uint reserved_1;

		public uint reserved_2;

		public uint reserved_3;

		public byte env_data_cnt_start;

		public byte block_cnt_start;

		public byte old_block_idx_start;

		public byte trx_block_idx_start;

		public byte env_data_cnt_end;

		public byte block_cnt_end;

		public byte old_block_idx_end;

		public byte trx_block_idx_end;

		public byte dbs_env_data_cnt_start;

		public byte dbs_block_cnt_start;

		public byte dbs_old_block_idx_start;

		public byte dbs_trx_block_idx_start;

		public byte dbs_env_data_cnt_end;

		public byte dbs_block_cnt_end;

		public byte dbs_old_block_idx_end;

		public byte dbs_trx_block_idx_end;

		public int uniqueRef;

		public ED_D_EnvBlock envBlock;

		public ED_D_EnvBlock envBlockODBSGrp;

		public ED_D_Event eventDescr;

		public ED_D_Code code0;

		public ED_D_Code code1;

		public ED_D_Code code2;

		public ED_D_Code code3;

		public int outputListInx;

		public byte[] envDataArr;

		public byte[] dbs_envDataArr;

		public ED_V_Data()
		{
			event_id = 0;
		}
	}
}
