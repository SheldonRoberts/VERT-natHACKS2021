using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Accord;
using brainflow;
using brainflow;

public class SimpleGetData : MonoBehaviour
{
    private BoardShim board_shim = null;
    private MLModel concentration = null;
    private int sampling_rate = 0;
    private int[] eeg_channels = null;
    private string data_string = "";
    private List<double> numbers = new List<double>();

    void Start()
    {
        for (int i =0; i < 20; i++)
        {
            numbers.Add(0);
        }

        try
        {
            //BoardShim.set_log_file("brainflow_log.txt");
            BoardShim.enable_dev_board_logger();

            BrainFlowInputParams input_params = new BrainFlowInputParams();
            input_params.serial_port = "COM6";
            int board_id = 1; // The id for a ganglion
            board_shim = new BoardShim(board_id, input_params);
            board_shim.prepare_session();
            board_shim.start_stream(450000, "file://brainflow_data.csv:w");
            BrainFlowModelParams concentration_params = new BrainFlowModelParams((int)BrainFlowMetrics.CONCENTRATION, (int)BrainFlowClassifiers.LDA);
            concentration = new MLModel(concentration_params);
            concentration.prepare();

            sampling_rate = BoardShim.get_sampling_rate(board_id);
            eeg_channels = BoardShim.get_eeg_channels(board_id);
            Debug.Log("Brainflow streaming was started");
        }
        catch (BrainFlowException e)
        {
            Debug.Log(e);
        }
    }

    void Update()
    {
        
        if ((board_shim == null) || (concentration == null))
        {
            return;
        }
        int number_of_data_points = sampling_rate * 10; // 10 second window for concentration measurement
        double[,] data = board_shim.get_current_board_data(number_of_data_points);
        //Debug.Log(data.GetLength(0);
        if (data.GetLength(1) < number_of_data_points)
        {
            return; // wait for more data to arrive
        }
        //prepare feature vector
        Tuple<double[], double[]> bands = DataFilter.get_avg_band_powers (data, eeg_channels, sampling_rate, true);
        double[] feature_vector = new double[bands.Item1.Length + bands.Item2.Length];
        bands.Item1.CopyTo(feature_vector, 0);
        bands.Item2.CopyTo(feature_vector, bands.Item1.Length);
        double value = concentration.predict(feature_vector);
        Debug.Log("Concentration: " + value);
        double height = 0;
        if (value > 0.55)
        {
            value = 1;
        } else
        {
            value = 0;
        }

        numbers.Add(value);
        numbers.RemoveAt(0);

        int total = 0;
        for (int i = 0; i < 20; i++)
        {
            total = total + (int)numbers[i];
        }
        float focusPercent = total / 20;

        if (focusPercent >= 0.90 )
        {
            data_string = "True";
        } else
        {
            data_string = "False";
        }
    }

    public string GetData()
    {
        return data_string;
    }

    private void OnDestroy()
    {
        if (board_shim != null)
        {
            try
            {
                board_shim.release_session();
                concentration.release();
            }
            catch (BrainFlowException e)
            {
                Debug.Log(e);
            }
            Debug.Log("Brainflow streaming was stopped");
        }
    }
}