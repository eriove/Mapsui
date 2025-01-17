using System;
using System.Linq;
using Mapsui.Extensions;
using Mapsui.Geometries;
using Mapsui.Layers;

namespace Mapsui.GeometryLayer
{
    public class GeometryFeature : BaseFeature, IGeometryFeature, IDisposable
    {
        private bool _disposed;

        public GeometryFeature()
        {
        }

        public GeometryFeature(IGeometryFeature feature)
        {
            Geometry = feature.Geometry;
            RenderedGeometry = feature.RenderedGeometry.ToDictionary(entry => entry.Key,
                entry => entry.Value);
            Styles = feature.Styles.ToList();
            foreach (var field in feature.Fields)
                this[field] = feature[field];
        }

        public IGeometry Geometry { get; set; }

        public MRect? Extent => Geometry?.BoundingBox.ToMRect(); // Todo: Make not-nullable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~GeometryFeature()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                foreach (var keyValuePair in RenderedGeometry)
                {
                    var disposable = keyValuePair.Value as IDisposable;
                    disposable?.Dispose();
                }
                RenderedGeometry.Clear();
            }
            _disposed = true;
        }
    }
}
