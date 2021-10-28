using MimeDetective;
using System;

namespace Module.Util.Extensao
{
    /// <summary>
    /// Extensão para obter dados de imagem em serviços api restfull
    /// </summary>
    public static class ByteArrayExtension
    {
        /// <summary>
        /// Obtém extensão da imagem a partir de um array de bytes
        /// </summary>
        /// <param name="file">Array de bytes</param>
        /// <returns>Extensão da imagem</returns>
        public static string ObterExtensao(this byte[] file)
        {
            FileType fileType = file.GetFileType();

            return fileType.Extension;
        }

        /// <summary>
        /// Obtém mime type da imagem a partir de um array de bytes
        /// </summary>
        /// <param name="file">Array de bytes</param>
        /// <returns>MimeType da imagem</returns>
        public static string ObterMimeType(this byte[] file)
        {
            FileType fileType = file.GetFileType();

            return fileType.Mime;
        }

        /// <summary>
        /// Converte o array de dados para o formato base64 para facilitar exibição json
        /// </summary>
        /// <param name="byteArray">Array de dados da imagem</param>
        /// <param name="contentType">Myme type da imagem</param>
        /// <returns>Imagem base 64</returns>
        public static string ToHtmlBase64(this byte[] byteArray)
        {
            var contentType = byteArray.ObterMimeType();
            var strArray = Convert.ToBase64String(byteArray);
            var urlArray = $"data:{contentType};base64,{strArray}";

            return urlArray;
        }
    }
}