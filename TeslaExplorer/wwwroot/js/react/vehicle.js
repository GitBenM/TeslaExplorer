import React from "react";
import ReactDOM from "react-dom";
import { Container, Col, Row } from 'react-bootstrap';

class Vehicle extends React.Component {
    constructor(props) {
        super(props);
        this.state = { vehicleData: props.data};
    }

    handleClick(i) {
    }

    render() {
        return (
            <Container>
                <Row>
                    <Col>{this.state.vehicleData.charge_state.battery_level}</Col>
                    <Col>2 of 2</Col>
                </Row>
                <Row>
                    <Col>1 of 3</Col>
                    <Col>2 of 3</Col>
                    <Col>3 of 3</Col>
                </Row>
            </Container>
        );
    }
}

ReactDOM.render(<Vehicle data={window.vehicleData} />, document.getElementById("vehicle-app"));