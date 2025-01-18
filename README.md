# Readme

## Applications
- **MatrixAlg** => build and analyze decompositions of transversals
- **CubeAlg** => build invariant cubes
- **MatrixDiagAlg** => build diagonal transversals
- **MosaicAlg** => build mosaic images
- **AbsolutelySymmetricCubeAlg** => build symmetric cube by adding points
- **PFunctionAlg** => calculate p and p+ functions of matrix
- **PFunctionAnalyzerAlg** => analyze p and p+ functions of matrix

### To be implemented
- Semisymmetrant

## Recommended configuration
`MaxParallelization`
- use amount of P-cores in **MatrixAlg**, **CubeAlg**, **PFunctionAnalyzerAlg**
- use amount of all cores **MatrixDiagAlg**, **MosaicAlg**
- not used in **AbsolutelySymmetricCubeAlg**, **PFunctionAlg**

## About
**Author**: N. V. Perepichka  
Implemented in scope of PhD study  

## Implementation
* C#
* .NET 8

## Build
[![build](https://github.com/nperepichka/MatrixAlg/actions/workflows/build.yml/badge.svg)](https://github.com/nperepichka/MatrixAlg/actions/workflows/build.yml)