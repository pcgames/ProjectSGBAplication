using System.Collections.Generic;
using System.Linq;

namespace DecoderSGB.Transformation
{
    public class PseudorandomSequence
    {
        public static void GetSequensies(int N, out List<double> I, out List<double> Q)
        {
            I = (new double[N]).ToList();
            Q = (new double[N]).ToList();

            FormSequanseI(I);
            FormSequanseQ(Q);
        }

        public static void GetSequensies_2chipsPerBit(int sequenseLength, out List<double> PRN_i_doubled, out List<double> PRN_q_doubled)
        {
            // Формирование ПСП с 2 отсчётами на чип
            List<double> PRN_seq_i = new List<double>();
            List<double> PRN_seq_q = new List<double>();

            GetSequensies(sequenseLength, out PRN_seq_i, out PRN_seq_q);

            PRN_i_doubled = new List<double>();         // ПСП с 2-мя отсчётами на чип, причем "1" и "0" стали "-1" и "1" соответственно.
            PRN_q_doubled = new List<double>();         // ПСП с 2-мя отсчётами на чип, -//-

            for (int i = 0; i < sequenseLength; i++)
            {
                double buff_i = 1.0 - 2.0 * PRN_seq_i[i];
                PRN_i_doubled.Add(buff_i);
                PRN_i_doubled.Add(buff_i);

                double buff_q = 1.0 - 2.0 * PRN_seq_q[i];
                PRN_q_doubled.Add(buff_q);
                PRN_q_doubled.Add(buff_q);
            }
        }
        public static void GetSequensies_3chipsPerBit(int sequenseLength, out List<double> PRN_i_doubled, out List<double> PRN_q_doubled)
        {
            // Формирование ПСП с 2 отсчётами на чип
            List<double> PRN_seq_i = new List<double>();
            List<double> PRN_seq_q = new List<double>();

            GetSequensies(sequenseLength, out PRN_seq_i, out PRN_seq_q);

            PRN_i_doubled = new List<double>();         // ПСП с 2-мя отсчётами на чип, причем "1" и "0" стали "-1" и "1" соответственно.
            PRN_q_doubled = new List<double>();         // ПСП с 2-мя отсчётами на чип, -//-

            for (int i = 0; i < sequenseLength; i++)
            {
                double buff_i = 1.0 - 2.0 * PRN_seq_i[i];
                PRN_i_doubled.Add(buff_i);
                PRN_i_doubled.Add(buff_i);
                PRN_i_doubled.Add(buff_i);

                double buff_q = 1.0 - 2.0 * PRN_seq_q[i];
                PRN_q_doubled.Add(buff_q);
                PRN_q_doubled.Add(buff_q);
                PRN_q_doubled.Add(buff_q);
            }
        }
        public static void GetSequensies_4chipsPerBit(int sequenseLength, out List<double> PRN_i_doubled, out List<double> PRN_q_doubled)
        {
            // Формирование ПСП с 2 отсчётами на чип
            List<double> PRN_seq_i = new List<double>();
            List<double> PRN_seq_q = new List<double>();

            GetSequensies(sequenseLength, out PRN_seq_i, out PRN_seq_q);

            PRN_i_doubled = new List<double>();         // ПСП с 2-мя отсчётами на чип, причем "1" и "0" стали "-1" и "1" соответственно.
            PRN_q_doubled = new List<double>();         // ПСП с 2-мя отсчётами на чип, -//-

            for (int i = 0; i < sequenseLength; i++)
            {
                double buff_i = 1.0 - 2.0 * PRN_seq_i[i];
                PRN_i_doubled.Add(buff_i);
                PRN_i_doubled.Add(buff_i);
                PRN_i_doubled.Add(buff_i);
                PRN_i_doubled.Add(buff_i);

                double buff_q = 1.0 - 2.0 * PRN_seq_q[i];
                PRN_q_doubled.Add(buff_q);
                PRN_q_doubled.Add(buff_q);
                PRN_q_doubled.Add(buff_q);
                PRN_q_doubled.Add(buff_q);
            }
        }

        private static void FormSequanseI(List<double> InnerArray)
        {
            int[] a = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
            int[] c = new int[23];

            for (int i = 0; i < 23; i++)
            {
                c[i] = a[22 - i];
            }

            for (int i = 0; i < 23; i++)
            {
                if (i > InnerArray.Count - 1)
                {
                    break;
                }
                InnerArray[i] = c[i];
            }
            for (int i = 0; i < InnerArray.Count - 23; i++)
            {
                InnerArray[i + 23] = InnerArray[i] + InnerArray[i + 18];
                if (InnerArray[i + 23] > 1)
                {
                    InnerArray[i + 23] = 0;
                }
            }
        }

        private static void FormSequanseQ(List<double> InnerArray)
        {
            int[] a = new int[] { 0, 0, 1, 1, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 0 };
            int[] c = new int[23];

            for (int i = 0; i < 23; i++)
            {
                c[i] = a[22 - i];
            }

            for (int i = 0; i < 23; i++)
            {
                InnerArray[i] = c[i];
            }
            for (int i = 0; i < InnerArray.Count - 23; i++)
            {
                InnerArray[i + 23] = InnerArray[i] + InnerArray[i + 18];
                if (InnerArray[i + 23] > 1)
                {
                    InnerArray[i + 23] = 0;
                }
            }
        }
    }
}
