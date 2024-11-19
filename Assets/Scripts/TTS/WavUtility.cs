using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace TTS
{ 
	public class WavUtility
	{
		const int BlockSize_16Bit = 2;

		
		
		public static AudioClip ToAudioClip(byte[] wavData, string name = "tempClip")
		{
			AudioClip audioClip = AudioClip.Create(name, wavData.Length / 2, 1, 44100, false);
			float[] samples = new float[wavData.Length / 2];
			for (int i = 0; i < samples.Length; i++)
			{
				samples[i] = (short)((wavData[i * 2 + 1] << 8) | wavData[i * 2]) / 32768.0f;
			}
			audioClip.SetData(samples, 0);
			return audioClip;
		}
    
		public static AudioClip ToAudioClip(byte[] wavData, int offset, int length, int freq)
		{
			float[] samples = ConvertBytesToSamples(wavData, offset, length);
			AudioClip audioClip = AudioClip.Create("tempClip", samples.Length, 1, freq, false);
			audioClip.SetData(samples, 0);
			return audioClip;
		}

		private static float[] ConvertBytesToSamples(byte[] bytes, int offset, int length)
		{
			float[] samples = new float[length / 2];
			for (int i = 0; i < length / 2; i++)
			{
				short sample = (short)((bytes[offset + i * 2 + 1] << 8) | bytes[offset + i * 2]);
				samples[i] = sample / 32768f; // Convertir a rango [-1, 1]
			}
			return samples;
		}
		public static AudioClip ToAudioClip (byte[] fileBytes, int offsetSamples = 0, string name = "wav")
		{
			int subchunk1 = BitConverter.ToInt32 (fileBytes, 16);
			UInt16 audioFormat = BitConverter.ToUInt16 (fileBytes, 20);
			string formatCode = FormatCode (audioFormat);
			Debug.AssertFormat (audioFormat == 1 || audioFormat == 65534, "Detected format code '{0}' {1}, but only PCM and WaveFormatExtensable uncompressed formats are currently supported.", audioFormat, formatCode);

			UInt16 channels = BitConverter.ToUInt16 (fileBytes, 22);
			int sampleRate = BitConverter.ToInt32 (fileBytes, 24);
			//int byteRate = BitConverter.ToInt32 (fileBytes, 28);
			//UInt16 blockAlign = BitConverter.ToUInt16 (fileBytes, 32);
			UInt16 bitDepth = BitConverter.ToUInt16 (fileBytes, 34);

			int headerOffset = 16 + 4 + subchunk1 + 4;
			int subchunk2 = BitConverter.ToInt32 (fileBytes, headerOffset);
			//Debug.LogFormat ("riff={0} wave={1} subchunk1={2} format={3} channels={4} sampleRate={5} byteRate={6} blockAlign={7} bitDepth={8} headerOffset={9} subchunk2={10} filesize={11}", riff, wave, subchunk1, formatCode, channels, sampleRate, byteRate, blockAlign, bitDepth, headerOffset, subchunk2, fileBytes.Length);
			float[] data;
			switch (bitDepth) {
				case 8:
					data = Convert8BitByteArrayToAudioClipData (fileBytes, headerOffset, subchunk2);
					break;
				case 16:
					data = Convert16BitByteArrayToAudioClipData (fileBytes, headerOffset, subchunk2);
					break;
				case 24:
					data = Convert24BitByteArrayToAudioClipData (fileBytes, headerOffset, subchunk2);
					break;
				case 32:
					data = Convert32BitByteArrayToAudioClipData (fileBytes, headerOffset, subchunk2);
					break;
				default:
					throw new Exception (bitDepth + " bit depth is not supported.");
			}

			AudioClip audioClip = AudioClip.Create (name, data.Length, (int)channels, sampleRate, false);
			audioClip.SetData (data, 0);
			return audioClip;
		}

		#region wav file bytes to Unity AudioClip conversion methods

		private static float[] Convert8BitByteArrayToAudioClipData (byte[] source, int headerOffset, int dataSize)
		{
			int wavSize = BitConverter.ToInt32 (source, headerOffset);
			headerOffset += sizeof(int);
			Debug.AssertFormat (wavSize > 0 && wavSize == dataSize, "Failed to get valid 8-bit wav size: {0} from data bytes: {1} at offset: {2}", wavSize, dataSize, headerOffset);

			float[] data = new float[wavSize];

			sbyte maxValue = sbyte.MaxValue;

			int i = 0;
			while (i < wavSize) {
				data [i] = (float)source [i] / maxValue;
				++i;
			}

			return data;
		}
		private static float[] Convert16BitByteArrayToAudioClipData (byte[] source, int headerOffset, int dataSize)
		{
			int wavSize = BitConverter.ToInt32 (source, headerOffset);
			headerOffset += sizeof(int);
			Debug.AssertFormat (wavSize > 0 && wavSize == dataSize, "Failed to get valid 16-bit wav size: {0} from data bytes: {1} at offset: {2}", wavSize, dataSize, headerOffset);

			int x = sizeof(Int16); // block size = 2
			int convertedSize = wavSize / x;

			float[] data = new float[convertedSize];

			Int16 maxValue = Int16.MaxValue;

			int offset = 0;
			int i = 0;
			while (i < convertedSize) {
				offset = i * x + headerOffset;
				data [i] = (float)BitConverter.ToInt16 (source, offset) / maxValue;
				++i;
			}
			Debug.AssertFormat (data.Length == convertedSize, "AudioClip .wav data is wrong size: {0} == {1}", data.Length, convertedSize);
			return data;
		}
		private static float[] Convert24BitByteArrayToAudioClipData (byte[] source, int headerOffset, int dataSize)
		{
			int wavSize = BitConverter.ToInt32 (source, headerOffset);
			headerOffset += sizeof(int);
			Debug.AssertFormat (wavSize > 0 && wavSize == dataSize, "Failed to get valid 24-bit wav size: {0} from data bytes: {1} at offset: {2}", wavSize, dataSize, headerOffset);

			int x = 3; // block size = 3
			int convertedSize = wavSize / x;

			int maxValue = Int32.MaxValue;

			float[] data = new float[convertedSize];

			byte[] block = new byte[sizeof(int)]; // usando un bloque de byte de 4 para copiar 3 bytes y luego copiar con 1 offset

			int offset = 0;
			int i = 0;
			while (i < convertedSize) {
				offset = i * x + headerOffset;
				Buffer.BlockCopy (source, offset, block, 1, x);
				data [i] = (float)BitConverter.ToInt32 (block, 0) / maxValue;
				++i;
			}

			Debug.AssertFormat (data.Length == convertedSize, "AudioClip .wav data is wrong size: {0} == {1}", data.Length, convertedSize);

			return data;
		}

		private static float[] Convert32BitByteArrayToAudioClipData (byte[] source, int headerOffset, int dataSize)
		{
			int wavSize = BitConverter.ToInt32 (source, headerOffset);
			headerOffset += sizeof(int);
			Debug.AssertFormat (wavSize > 0 && wavSize == dataSize, "Failed to get valid 32-bit wav size: {0} from data bytes: {1} at offset: {2}", wavSize, dataSize, headerOffset);

			int x = sizeof(float); //  block size = 4
			int convertedSize = wavSize / x;

			Int32 maxValue = Int32.MaxValue;

			float[] data = new float[convertedSize];

			int offset = 0;
			int i = 0;
			while (i < convertedSize) {
				offset = i * x + headerOffset;
				data [i] = (float)BitConverter.ToInt32 (source, offset) / maxValue;
				++i;
			}

			Debug.AssertFormat (data.Length == convertedSize, "AudioClip .wav data is wrong size: {0} == {1}", data.Length, convertedSize);

			return data;
		}

		#endregion
		
		public static byte[] FromAudioClip (AudioClip audioClip, out string filepath, bool saveAsFile = true, string dirname = "recordings")
		{
			MemoryStream stream = new MemoryStream ();
			const int headerSize = 44;
			UInt16 bitDepth = 16; //BitDepth (audioClip);

			int fileSize = audioClip.samples * BlockSize_16Bit + headerSize; // BlockSize (bitDepth)
			WriteFileHeader (ref stream, fileSize);
			WriteFileFormat (ref stream, audioClip.channels, audioClip.frequency, bitDepth);
			WriteFileData (ref stream, audioClip, bitDepth);
			byte[] bytes = stream.ToArray ();
			Debug.AssertFormat (bytes.Length == fileSize, "Unexpected AudioClip to wav format byte count: {0} == {1}", bytes.Length, fileSize);
			if (saveAsFile) {
				filepath = string.Format ("{0}/{1}/{2}.{3}", Application.persistentDataPath, dirname, DateTime.UtcNow.ToString ("yyMMdd-HHmmss-fff"), "wav");
				Directory.CreateDirectory (Path.GetDirectoryName (filepath));
				File.WriteAllBytes (filepath, bytes);
			} else {
				filepath = null;
			}

			stream.Dispose ();

			return bytes;
		}

		#region write .wav file functions

		private static int WriteFileHeader (ref MemoryStream stream, int fileSize)
		{
			int count = 0;
			int total = 12;
			byte[] riff = Encoding.ASCII.GetBytes ("RIFF");
			count += WriteBytesToMemoryStream (ref stream, riff, "ID");
			int chunkSize = fileSize - 8; 
			count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (chunkSize), "CHUNK_SIZE");
			byte[] wave = Encoding.ASCII.GetBytes ("WAVE");
			count += WriteBytesToMemoryStream (ref stream, wave, "FORMAT");
			Debug.AssertFormat (count == total, "Unexpected wav descriptor byte count: {0} == {1}", count, total);
			return count;
		}

		private static int WriteFileFormat (ref MemoryStream stream, int channels, int sampleRate, UInt16 bitDepth)
		{
			int count = 0;
			int total = 24;
			byte[] id = Encoding.ASCII.GetBytes ("fmt ");
			count += WriteBytesToMemoryStream (ref stream, id, "FMT_ID");
			int subchunk1Size = 16; // 24 - 8
			count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (subchunk1Size), "SUBCHUNK_SIZE");
			UInt16 audioFormat = 1;
			count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (audioFormat), "AUDIO_FORMAT");
			UInt16 numChannels = Convert.ToUInt16 (channels);
			count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (numChannels), "CHANNELS");
			count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (sampleRate), "SAMPLE_RATE");
			int byteRate = sampleRate * channels * BytesPerSample (bitDepth);
			count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (byteRate), "BYTE_RATE");
			UInt16 blockAlign = Convert.ToUInt16 (channels * BytesPerSample (bitDepth));
			count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (blockAlign), "BLOCK_ALIGN");
			count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (bitDepth), "BITS_PER_SAMPLE");
			Debug.AssertFormat (count == total, "Unexpected wav fmt byte count: {0} == {1}", count, total);
			return count;
		}

		private static int WriteFileData (ref MemoryStream stream, AudioClip audioClip, UInt16 bitDepth)
		{
			int count = 0;
			int total = 8;
			// Copy float[] data from AudioClip
			float[] data = new float[audioClip.samples * audioClip.channels];
			audioClip.GetData (data, 0);
			byte[] bytes = ConvertAudioClipDataToInt16ByteArray (data);
			byte[] id = Encoding.ASCII.GetBytes ("data");
			count += WriteBytesToMemoryStream (ref stream, id, "DATA_ID");
			int subchunk2Size = Convert.ToInt32 (audioClip.samples * BlockSize_16Bit); // BlockSize (bitDepth)
			count += WriteBytesToMemoryStream (ref stream, BitConverter.GetBytes (subchunk2Size), "SAMPLES");
			Debug.AssertFormat (count == total, "Unexpected wav data id byte count: {0} == {1}", count, total);
			count += WriteBytesToMemoryStream (ref stream, bytes, "DATA");
			Debug.AssertFormat (bytes.Length == subchunk2Size, "Unexpected AudioClip to wav subchunk2 size: {0} == {1}", bytes.Length, subchunk2Size);

			return count;
		}

		private static byte[] ConvertAudioClipDataToInt16ByteArray (float[] data)
		{
			MemoryStream dataStream = new MemoryStream ();
			int x = sizeof(Int16);
			Int16 maxValue = Int16.MaxValue;
			int i = 0;
			while (i < data.Length) {
				dataStream.Write (BitConverter.GetBytes (Convert.ToInt16 (data [i] * maxValue)), 0, x);
				++i;
			}
			byte[] bytes = dataStream.ToArray ();
			Debug.AssertFormat (data.Length * x == bytes.Length, "Unexpected float[] to Int16 to byte[] size: {0} == {1}", data.Length * x, bytes.Length);
			dataStream.Dispose ();
			return bytes;
		}

		private static int WriteBytesToMemoryStream (ref MemoryStream stream, byte[] bytes, string tag = "")
		{
			int count = bytes.Length;
			stream.Write (bytes, 0, count);
			//Debug.LogFormat ("WAV:{0} wrote {1} bytes.", tag, count);
			return count;
		}

		#endregion

		public static UInt16 BitDepth (AudioClip audioClip)
		{
			UInt16 bitDepth = Convert.ToUInt16 (audioClip.samples * audioClip.channels * audioClip.length / audioClip.frequency);
			Debug.AssertFormat (bitDepth == 8 || bitDepth == 16 || bitDepth == 32, "Unexpected AudioClip bit depth: {0}. Expected 8 or 16 or 32 bit.", bitDepth);
			return bitDepth;
		}

		private static int BytesPerSample (UInt16 bitDepth)
		{
			return bitDepth / 8;
		}
		
		private static string FormatCode (UInt16 code)
		{
			switch (code) {
				case 1:
					return "PCM";
				case 2:
					return "ADPCM";
				case 3:
					return "IEEE";
				case 7:
					return "μ-law";
				case 65534:
					return "WaveFormatExtensable";
				default:
					Debug.LogWarning ("Unknown wav code format:" + code);
					return "";
			}
		}

	}
}