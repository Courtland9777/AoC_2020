using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AoC_2020.Helpers
{
    public static class DoorHelper
    {
        public static BigInteger GetEncryptionKey(
            BigInteger publicKeyDoor,
            BigInteger publicKeyCard)
        {
            var loopSizeDoor = GetLoopSizeFromPublicKey(publicKeyDoor);
            var loopSizeCard = GetLoopSizeFromPublicKey(publicKeyCard);

            var encryptionKeyDoor = GetTransformedSubjectNumber(loopSizeDoor, publicKeyCard);
            var encryptionKeyCard = GetTransformedSubjectNumber(loopSizeCard, publicKeyDoor);

            if (encryptionKeyCard != encryptionKeyDoor)
            {
                throw new Exception($"Encryption keys don't match: card: {encryptionKeyCard}, door: {encryptionKeyDoor}");
            }

            return encryptionKeyCard;
        }

        public static int GetLoopSizeFromPublicKey(BigInteger publicKey)
        {
            int result = 1;
            BigInteger currentTransform = 1;
            while (true)
            {
                currentTransform = (currentTransform * 7) % 20201227;
                if (currentTransform == publicKey)
                {
                    break;
                }
                result++;
            }
            return result;
        }

        public static BigInteger GetTransformedSubjectNumber(int loopSize, BigInteger subjectNumber)
        {
            BigInteger result = 1;
            for (int i = 0; i < loopSize; i++)
            {
                result = (result * subjectNumber) % 20201227;
            }
            return result;
        }
    }
}
