using System;
using System.IO;
using System.Security.Cryptography;

public static class LocalDataChecker
{
    public static string ComputeChecksum(string filePath)
    {
        // ������ �б� ����(FileMode.Open)���� ���� FileStream ��ü�� ����
        using (FileStream stream = File.OpenRead(filePath))
        {
            // SHA256 �ؽ� �˰����� ����ϱ� ���� SHA256 �ν��Ͻ��� ����
            using (SHA256 sha256 = SHA256.Create())
            {
                // ���� ��Ʈ���� �����͸� �о SHA256 �ؽø� ���
                byte[] hash = sha256.ComputeHash(stream);

                // �ؽ� ���� 16���� ���ڿ��� ��ȯ�Ͽ� ��ȯ
                // BitConverter.ToString()�� ����Ʈ �迭�� 16���� ���ڿ��� ��ȯ�ϰ�, Replace("-", "")�� ���ڿ����� "-"�� ������
                // ToLowerInvariant()�� ���ڿ��� �ҹ��ڷ� ��ȯ�Ͽ� ��ȯ
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}