using System;
using System.IO;
using System.Security.Cryptography;

public static class LocalDataChecker
{
    public static string ComputeChecksum(string filePath)
    {
        // 파일을 읽기 전용(FileMode.Open)으로 열고 FileStream 객체를 생성
        using (FileStream stream = File.OpenRead(filePath))
        {
            // SHA256 해시 알고리즘을 사용하기 위해 SHA256 인스턴스를 생성
            using (SHA256 sha256 = SHA256.Create())
            {
                // 파일 스트림의 데이터를 읽어서 SHA256 해시를 계산
                byte[] hash = sha256.ComputeHash(stream);

                // 해시 값을 16진수 문자열로 변환하여 반환
                // BitConverter.ToString()은 바이트 배열을 16진수 문자열로 변환하고, Replace("-", "")는 문자열에서 "-"를 제거함
                // ToLowerInvariant()는 문자열을 소문자로 변환하여 반환
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}